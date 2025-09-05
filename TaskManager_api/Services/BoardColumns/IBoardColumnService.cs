using TaskManager_api.DTOs.BoardColumn;

namespace TaskManager_api.Services.BoardColumns
{
    
        public interface IBoardColumnService
        {
            Task<BoardColumnResponseDTO> AddColumnAsync(int boardId, BoardColumnCreateDTO dto);
            Task<bool> UpdateColumnAsync(int columnId, BoardColumnCreateDTO dto);
            Task<bool> ArchiveColumnAsync(int columnId);
            Task<bool> UnarchiveColumnAsync(int columnId);
            Task<bool> DeleteColumnPermanentlyAsync(int columnId);
        }

    
}
