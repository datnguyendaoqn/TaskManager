using Microsoft.EntityFrameworkCore;
using TaskManager_api.DTOs.BoardColumn;
using TaskManager_api.Models;
using TaskManager_api.Repositories.BoardColumns;

namespace TaskManager_api.Services.BoardColumns
{
    public class BoardColumnService : IBoardColumnService
    {
        private readonly IBoardColumnRepository _columnRepo;

        public BoardColumnService(IBoardColumnRepository columnRepo)
        {
            _columnRepo = columnRepo;
        }

        public async Task<BoardColumnResponseDTO> AddColumnAsync(int boardId, BoardColumnCreateDTO dto)
        {
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

        public async Task<bool> UpdateColumnAsync(int columnId, BoardColumnCreateDTO dto)
        {
            var column = await _columnRepo.GetByIdAsync(columnId);
            if (column == null) return false;

            column.Name = dto.Name;

            if (dto.Position.HasValue && dto.Position.Value != column.Position)
            {
                // Lấy column đang chiếm vị trí mới
                var targetColumn = await _columnRepo.GetByBoardIdAndPositionAsync(column.BoardId, dto.Position.Value);
                if (targetColumn != null)
                {
                    // Swap position
                    targetColumn.Position = column.Position;
                    targetColumn.UpdatedAt = DateTime.UtcNow;
                }

                column.Position = dto.Position.Value;
            }

            column.UpdatedAt = DateTime.UtcNow;

            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveColumnAsync(int columnId)
        {
            var column = await _columnRepo.GetByIdAsync(columnId);
            if (column == null) return false;

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

        public async Task<bool> UnarchiveColumnAsync(int columnId)
        {
            var column = await _columnRepo.GetByIdAsync(columnId, includeArchived: true);
            if (column == null) return false;

            column.IsArchived = false;
            column.UpdatedAt = DateTime.UtcNow;

            // Thêm vào cuối cùng position
            var maxPosition = await _columnRepo.GetMaxPositionAsync(column.BoardId);
            column.Position = maxPosition + 1;

            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteColumnPermanentlyAsync(int columnId)
        {
            var column = await _columnRepo.GetByIdAsync(columnId, includeArchived: true);
            if (column == null) return false;

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

        public async Task<IEnumerable<BoardColumnResponseDTO>> GetArchivedColumnsAsync(int boardId)
        {
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


    
