using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using TaskManager_api.Data;
using TaskManager_api.DTOs.BoardColumn;
using TaskManager_api.Models;
using TaskManager_api.Repositories.BoardColumns;
using TaskManager_api.Repositories.Boards;
using TaskManager_api.Repositories.ProjectUsers;

namespace TaskManager_api.Services.BoardColumns
{
    public class BoardColumnService : IBoardColumnService
    {
        private readonly IBoardColumnRepository _columnRepo;
        private readonly AppDbContext _context;
        private readonly IProjectUserRepository _projectUserRepo;
        private readonly IBoardRepository _boardRepository;

        public BoardColumnService(IBoardColumnRepository columnRepo,AppDbContext context,IProjectUserRepository projectUserRepo,IBoardRepository boardRepo)
        {
            _columnRepo = columnRepo;
            _context = context;
            _projectUserRepo = projectUserRepo;
            _boardRepository = boardRepo;
        }
        public async Task<IEnumerable<BoardColumnResponseDTO>> GetColumnAsync(int boardId, int currentUser)
        {
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, board.ProjectId))
                throw new Exception("User has no access to this project");
            var cols = await _columnRepo.GetByBoardIdAsync(boardId);
            return cols.Select(c => new BoardColumnResponseDTO
            {
                ColumnId = c.ColumnId,
                Name = c.Name,
                Position = c.Position,
            });

        }
        public async Task<BoardColumnResponseDTO> AddColumnAsync(int boardId, BoardColumnCreateDTO dto,int currentUser)
        {
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, board.ProjectId))
                throw new Exception("User has no access to this project");
            if (dto.Name.IsNullOrEmpty())
                throw new Exception("column must has name");
            // Lấy max position hiện tại trong board
            var maxPosition = await _columnRepo.GetMaxPositionAsync(boardId);

            var column = new BoardColumn
            {
                BoardId = boardId,
                Name = dto.Name,
                Position = maxPosition + 1,  // tự động thêm vào cuối
                CreatedAt = DateTime.UtcNow
            };

            await _columnRepo.AddAsync(column);
            await _columnRepo.SaveChangesAsync();

            return new BoardColumnResponseDTO
            {
                ColumnId = column.ColumnId,
                Name = column.Name,
                Position = column.Position,
                IsArchived = column.IsArchived
            };
        }

        public async Task<bool> UpdateColumnAsync(int columnId, BoardColumnCreateDTO dto,int currentUser)
        {
            var column = await _columnRepo.GetByIdAsync(columnId);
            if (column == null) return false;
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, column.Board.ProjectId))
                throw new Exception("User has no access to this project");
          
            column.Name = dto.Name;
            column.UpdatedAt = DateTime.UtcNow;
            _context.BoardColumns.Update(column);
            await _columnRepo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> MoveColumnAsync(int columnId, int pos,int currentUser)
        {
            var column = await _columnRepo.GetByIdAsync(columnId);
            if (column == null) return false;
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, column.Board.ProjectId))
                throw new Exception("User has no access to this project");

            var columns = (await _columnRepo.GetByBoardIdAsync(column.BoardId)).ToList();
            int oldPos = column.Position;
            int newPos = Math.Clamp(pos, 1, columns.Count);

            if (oldPos == newPos) return true; // không thay đổi

            foreach (var c in columns)
            {
                if (c.ColumnId == columnId) continue;

                if (oldPos < newPos)
                {
                    if (c.Position > oldPos && c.Position <= newPos) c.Position -= 1;
                }
                else
                {
                    if (c.Position >= newPos && c.Position < oldPos) c.Position += 1;
                }
            }

            column.Position = newPos;
            column.UpdatedAt = DateTime.UtcNow;

            foreach (var c in columns)
            {
                _context.BoardColumns.Update(c);
            }
            _context.BoardColumns.Update(column);
                
            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveColumnAsync(int columnId,int currentUser)
        {
            var column = await _columnRepo.GetByIdAsync(columnId);
            if (column == null) return false;
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, column.Board.ProjectId))
                throw new Exception("User has no access to this project");

            column.IsArchived = true;
            column.UpdatedAt = DateTime.UtcNow;

            // Shift các column còn lại để lấp khoảng trống
            var boardColumns = await _columnRepo.GetByBoardIdAsync(column.BoardId, includeArchived: false);
            foreach (var c in boardColumns)
            {
                if (c.Position > column.Position)
                    c.Position -= 1;
            }

            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnarchiveColumnAsync(int columnId,int currentUser)
        {
            var column = await _columnRepo.GetByIdAsync(columnId, includeArchived: true);
            if (column == null) return false;
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, column.Board.ProjectId))
                throw new Exception("User has no access to this project");

            column.IsArchived = false;
            column.UpdatedAt = DateTime.UtcNow;

            // Thêm vào cuối cùng position
            var maxPosition = await _columnRepo.GetMaxPositionAsync(column.BoardId);
            column.Position = maxPosition + 1;

            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteColumnPermanentlyAsync(int columnId,int currentUser)
        {
            var column = await _columnRepo.GetByIdAsync(columnId, includeArchived: true);
            if (column == null) return false;
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, column.Board.ProjectId))
                throw new Exception("User has no access to this project");

            // Shift các column còn lại nếu không archive
            if (!column.IsArchived)
            {
                var boardColumns = await _columnRepo.GetByBoardIdAsync(column.BoardId, includeArchived: false);
                foreach (var c in boardColumns)
                {
                    if (c.Position > column.Position)
                        c.Position -= 1;
                }
            }

            await _columnRepo.RemoveAsync(column);
            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BoardColumnResponseDTO>> GetArchivedColumnsAsync(int boardId,int currentUser)
        {
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (!await _projectUserRepo.UserHasProjectAsync(currentUser, board.ProjectId))
                throw new Exception("User has no access to this project");
            var columns = await _columnRepo.GetArchivedByBoardIdAsync(boardId);
            return columns.Select(c => new BoardColumnResponseDTO
            {
                ColumnId = c.ColumnId,
                Name = c.Name,
                Position = c.Position,
                IsArchived = c.IsArchived
            });
        }
    }
}


    
