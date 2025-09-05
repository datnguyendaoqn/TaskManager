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
            var column = new BoardColumn
            {
                BoardId = boardId,
                Name = dto.Name,
                Position = dto.Position,
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
            column.Position = dto.Position;
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

            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnarchiveColumnAsync(int columnId)
        {
            var column = await _columnRepo.GetByIdAsync(columnId, includeArchived: true);
            if (column == null) return false;

            column.IsArchived = false;
            column.UpdatedAt = DateTime.UtcNow;

            await _columnRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteColumnPermanentlyAsync(int columnId)
        {
            var column = await _columnRepo.GetByIdAsync(columnId, includeArchived: true);
            if (column == null) return false;

            await _columnRepo.RemoveAsync(column);
            await _columnRepo.SaveChangesAsync();
            return true;
        }
    }


}
