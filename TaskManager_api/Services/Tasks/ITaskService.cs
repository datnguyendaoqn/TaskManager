using TaskManager_api.DTOs.Task;

namespace TaskManager_api.Services.Tasks
{
    public interface ITaskService
    {
        // CRUD
        Task<TaskDTO> CreateTaskAsync(int columnId, int createdBy, TaskCreateDTO dto);
        Task<TaskDTO?> GetTaskAsync(int taskId, int currentUserId);
        Task<IEnumerable<TaskDTO>> GetTasksByColumnAsync(int columnId, int currentUserId);
        Task<IEnumerable<TaskDTO>> GetTasksByBoardAsync(int boardId, int currentUserId);
        Task DeleteTaskAsync(int taskId, int currentUserId);
        Task<TaskDTO> UpdateTaskInfoAsync(int taskId, int currentUserId, TaskUpdateDTO dto);

        Task<TaskDTO> MoveTaskAsync(int taskId, int currentUserId, MoveTaskDTO dto);
        // Archive
        Task<TaskDTO> ArchiveTaskAsync(int taskId, int currentUserId);
        Task<TaskDTO> UnarchiveTaskAsync(int taskId, int currentUserId);
        Task<IEnumerable<TaskDTO>> GetArchivedTasksByBoardAsync(int boardId, int currentUserId);
    }
}
