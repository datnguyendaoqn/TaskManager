using TaskManager_api.DTOs.Board;
using TaskManager_api.DTOs.BoardColumn;
using TaskManager_api.Models;
using TaskManager_api.Repositories.Boards;

namespace TaskManager_api.Services.Boards
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepo;

        public BoardService(IBoardRepository boardRepo)
        {
            _boardRepo = boardRepo;
        }

        public async Task<BoardResponseDTO> CreateBoardAsync(int projectId, int userId, BoardCreateDTO dto)
        {
            var board = new Board
            {
                ProjectId = projectId,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                Columns = new List<BoardColumn>
            {
                new BoardColumn { Name = "To Do", Position = 1, CreatedAt = DateTime.UtcNow },
                new BoardColumn { Name = "In Progress", Position = 2, CreatedAt = DateTime.UtcNow },
                new BoardColumn { Name = "Done", Position = 3, CreatedAt = DateTime.UtcNow }
            }
            };

            await _boardRepo.AddAsync(board);
            await _boardRepo.SaveChangesAsync();

            return MapToResponse(board);
        }

        public async Task<BoardResponseDTO?> GetBoardDetailAsync(int boardId, bool includeArchived = false)
        {
            var board = await _boardRepo.GetByIdAsync(boardId, includeArchived);
            return board == null ? null : MapToResponse(board);
        }

        public async Task<IEnumerable<BoardResponseDTO>> GetBoardsOfProjectAsync(int projectId, bool includeArchived = false)
        {
            var boards = await _boardRepo.GetByProjectIdAsync(projectId, includeArchived);
            return boards.Select(MapToResponse);
        }

        public async Task<bool> ArchiveBoardAsync(int boardId, int userId)
        {
            var board = await _boardRepo.GetByIdAsync(boardId);
            if (board == null) return false;

            board.IsArchived = true;
            board.UpdatedAt = DateTime.UtcNow;
            await _boardRepo.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<BoardResponseDTO>> GetArchivedBoardsAsync(int projectId)
        {
            var boards = await _boardRepo.GetArchivedByProjectIdAsync(projectId);
            return boards.Select(MapToResponse);
        }

        public async Task<bool> UnarchiveBoardAsync(int boardId, int userId)
        {
            var board = await _boardRepo.GetByIdAsync(boardId, includeArchived: true);
            if (board == null) return false;

            board.IsArchived = false;
            board.UpdatedAt = DateTime.UtcNow;
            await _boardRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBoardPermanentlyAsync(int boardId, int userId)
        {
            var board = await _boardRepo.GetByIdAsync(boardId, includeArchived: true);
            if (board == null) return false;

            await _boardRepo.RemoveAsync(board);
            await _boardRepo.SaveChangesAsync();
            return true;
        }

        private BoardResponseDTO MapToResponse(Board board)
        {
            return new BoardResponseDTO
            {
                BoardId = board.BoardId,
                Name = board.Name,
                CreatedAt = board.CreatedAt,
                IsArchived = board.IsArchived,
                Columns = board.Columns
                    .Where(c => !c.IsArchived)
                    .OrderBy(c => c.Position)
                    .Select(c => new BoardColumnResponseDTO
                    {
                        ColumnId = c.ColumnId,
                        Name = c.Name,
                        Position = c.Position,
                        IsArchived = c.IsArchived
                    }).ToList()
            };
        }
    }


}
