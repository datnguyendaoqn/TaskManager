using TaskManager_api.Models;

namespace TaskManager_api.Repositories.BoardColumns
{
    public interface IBoardColumnRepository
    {
        Task AddAsync(BoardColumn column);
        Task<BoardColumn?> GetByIdAsync(int columnId, bool includeArchived = false);
        Task RemoveAsync(BoardColumn column);
        Task SaveChangesAsync();
    }
}
