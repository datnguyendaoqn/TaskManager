using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Boards
    
{
    public interface IBoardRepository
    {
        Task AddAsync(Board board);
        Task<Board?> GetByIdAsync(int boardId, bool includeArchived = false);
        Task<IEnumerable<Board>> GetByProjectIdAsync(int projectId, bool includeArchived = false);
        Task<IEnumerable<Board>> GetArchivedByProjectIdAsync(int projectId); 
        Task RemoveAsync(Board board);
        Task SaveChangesAsync();
    }

}
