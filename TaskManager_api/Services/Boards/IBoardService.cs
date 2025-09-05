using TaskManager_api.DTOs.Board;

namespace TaskManager_api.Services.Boards
{
    public interface IBoardService
    {
        Task<BoardResponseDTO> CreateBoardAsync(int projectId, int userId, BoardCreateDTO dto);
        Task<BoardResponseDTO?> GetBoardDetailAsync(int boardId, bool includeArchived = false);
        Task<IEnumerable<BoardResponseDTO>> GetBoardsOfProjectAsync(int projectId, bool includeArchived = false);
        Task<bool> ArchiveBoardAsync(int boardId, int userId);
        Task<bool> UnarchiveBoardAsync(int boardId, int userId);
        Task<bool> DeleteBoardPermanentlyAsync(int boardId, int userId);
    }

}
