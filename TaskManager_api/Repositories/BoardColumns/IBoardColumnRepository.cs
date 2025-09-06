using TaskManager_api.Models;

namespace TaskManager_api.Repositories.BoardColumns
{
    public interface IBoardColumnRepository
    {
        Task AddAsync(BoardColumn column);
        Task<BoardColumn?> GetByIdAsync(int columnId, bool includeArchived = false);
        Task<IEnumerable<BoardColumn>> GetArchivedByBoardIdAsync(int boardId);
        Task RemoveAsync(BoardColumn column);
        Task SaveChangesAsync();
        Task<int> GetMaxPositionAsync(int boardId);
        Task<IEnumerable<BoardColumn>> GetByBoardIdAsync(int boardId, bool includeArchived = false);
        Task<BoardColumn?> GetByBoardIdAndPositionAsync(int boardId, int position);


    }
}
