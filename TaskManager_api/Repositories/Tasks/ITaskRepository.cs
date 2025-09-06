using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Tasks
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetByBoardAsync(int boardId,bool includeArchived = false);
        Task<IEnumerable<TaskItem>> GetByColumnAsync(int columnId, bool includeArchived = false);
        Task<IEnumerable<TaskItem>> GetArchivedByBoardAsync(int boardId);
        Task<TaskItem?> GetByIdWithProjectCheckAsync(int taskId, int userId);

    }
}
