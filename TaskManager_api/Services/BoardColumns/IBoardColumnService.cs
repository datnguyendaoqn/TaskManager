using TaskManager_api.DTOs.BoardColumn;

namespace TaskManager_api.Services.BoardColumns
{
    
        public interface IBoardColumnService
        {
            Task<IEnumerable<BoardColumnResponseDTO>> GetColumnAsync(int boardId, int currentUser); 
            Task<BoardColumnResponseDTO> AddColumnAsync(int boardId, BoardColumnCreateDTO dto, int currentUser);
            Task<bool> UpdateColumnAsync(int columnId, BoardColumnCreateDTO dto, int currentUser);
            Task<bool> MoveColumnAsync(int columnId, int pos, int currentUser);
            Task<bool> ArchiveColumnAsync(int columnId, int currentUser);
            Task<bool> UnarchiveColumnAsync(int columnId, int currentUser);
            Task<bool> DeleteColumnPermanentlyAsync(int columnId, int currentUser);
            Task<IEnumerable<BoardColumnResponseDTO>> GetArchivedColumnsAsync(int boardId, int currentUser);
    }

    
}
