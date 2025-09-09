using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.DTOs.Task;
using TaskManager_api.Models;
using TaskManager_api.Repositories.BoardColumns;
using TaskManager_api.Repositories.Boards;
using TaskManager_api.Repositories.ProjectUsers;
using TaskManager_api.Repositories.Tasks;

namespace TaskManager_api.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly IBoardColumnRepository _columnRepo;
        private readonly IProjectUserRepository _projectUserRepo;
        private readonly IBoardRepository _boardRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(
            ITaskRepository repo,
            IBoardColumnRepository columnRepo,
            IProjectUserRepository projectUserRepo,
            IBoardRepository boardRepo,
            AppDbContext context,
            IMapper mapper)
        {
            _repo = repo;
            _columnRepo = columnRepo;
            _projectUserRepo = projectUserRepo;
            _boardRepo = boardRepo;
            _context = context;
            _mapper = mapper;
        }

        // ========== HELPERS ==========
        private async Task<TaskItem> GetTaskWithAccessCheckAsync(int taskId, int userId)
        {
            var task = await _repo.GetByIdWithProjectCheckAsync(taskId, userId);
            if (task == null) throw new Exception("Task not found or no access");
            return task;
        }

        private static void ReorderTasks(IEnumerable<TaskItem> tasks)
        {
            int pos = 1;
            foreach (var t in tasks.OrderBy(x => x.Position))
                t.Position = pos++;
        }

        private async Task<int> GetNextPositionAsync(int columnId)
        {
            return (await _context.Tasks
                .Where(t => t.ColumnId == columnId && !t.IsArchived)
                .MaxAsync(t => (int?)t.Position)) ?? 0 + 1;
        }

        private async Task<bool> HasProjectAccessAsync(int userId, int projectId) =>
            await _projectUserRepo.UserHasProjectAsync(userId, projectId);

        // ========== CREATE ==========
        public async Task<TaskDTO> CreateTaskAsync(int columnId, int createdBy, TaskCreateDTO dto)
        {
            var column = await _columnRepo.GetByIdAsync(columnId) ?? throw new Exception("Column not found");
            if (!await HasProjectAccessAsync(createdBy, column.Board.ProjectId))
                throw new Exception("User has no access to this project");

            var task = _mapper.Map<TaskItem>(dto);
            task.ColumnId = columnId;
            task.BoardId = column.BoardId;
            task.CreatedBy = createdBy;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            task.Position = await GetNextPositionAsync(columnId);

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }

        // ========== READ ==========
        public async Task<TaskDTO?> GetTaskAsync(int taskId, int currentUserId) =>
            _mapper.Map<TaskDTO>(await _repo.GetByIdWithProjectCheckAsync(taskId, currentUserId));

        public async Task<IEnumerable<TaskDTO>> GetTasksByColumnAsync(int columnId, int currentUserId)
        {
            var column = await _columnRepo.GetByIdAsync(columnId)
                         ?? throw new Exception("Column not found");

            if (!await HasProjectAccessAsync(currentUserId, column.Board.ProjectId))
                throw new Exception("User has no access to this project");

            var tasks = await _repo.GetByColumnAsync(columnId);
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksByBoardAsync(int boardId, int currentUserId)
        {
            var board = await _boardRepo.GetByIdAsync(boardId)
                        ?? throw new Exception("Board not found");

            if (!await HasProjectAccessAsync(currentUserId, board.ProjectId))
                throw new Exception("User has no access to this project");

            var tasks = await _repo.GetByBoardAsync(boardId);
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }

        // ========== UPDATE INFO ==========
        public async Task<TaskDTO> UpdateTaskInfoAsync(int taskId, int currentUserId, TaskUpdateDTO dto)
        {
            var task = await GetTaskWithAccessCheckAsync(taskId, currentUserId);
            _mapper.Map(dto, task);
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<TaskDTO>(task);
        }

        // ========== MOVE / REORDER ==========
        public async Task<TaskDTO> MoveTaskAsync(int taskId, int currentUserId, MoveTaskDTO dto)
        {
            var task = await GetTaskWithAccessCheckAsync(taskId, currentUserId);

            int oldColumnId = task.ColumnId;
            int oldPosition = task.Position;

            if (dto.NewColumnId.HasValue && dto.NewColumnId.Value != task.ColumnId)
            {
                int newColumnId = dto.NewColumnId.Value;

                // Column cũ: reorder các task còn lại
                var tasksInOldColumn = await _context.Tasks
                    .Where(t => t.ColumnId == oldColumnId && t.TaskId != task.TaskId && !t.IsArchived)
                    .OrderBy(t => t.Position)
                    .ToListAsync();
                ReorderTasks(tasksInOldColumn);

                // Column mới: lấy các task hiện tại
                var tasksInNewColumn = await _context.Tasks
                    .Where(t => t.ColumnId == newColumnId && !t.IsArchived)
                    .OrderBy(t => t.Position)
                    .ToListAsync();

                int newPos;
                if (dto.NewPosition.HasValue)
                {
                    newPos = Math.Clamp(dto.NewPosition.Value, 1, tasksInNewColumn.Count + 1);

                    // Dịch các task từ vị trí chèn trở đi xuống 1
                    foreach (var t in tasksInNewColumn.Where(t => t.Position >= newPos))
                        t.Position++;
                }
                else
                {
                    newPos = tasksInNewColumn.Count + 1;
                }

                task.ColumnId = newColumnId;
                task.Position = newPos;

                _context.Tasks.UpdateRange(tasksInOldColumn);
                _context.Tasks.UpdateRange(tasksInNewColumn);
            }
            else if (dto.NewPosition.HasValue && dto.NewPosition.Value != task.Position)
            {
                // Trong cùng column: swap pos
                var tasksInColumn = await _context.Tasks
                    .Where(t => t.ColumnId == task.ColumnId && !t.IsArchived && t.TaskId != task.TaskId)
                    .ToListAsync();

                int targetPos = Math.Clamp(dto.NewPosition.Value, 1, tasksInColumn.Count + 1);

                var taskAtTarget = tasksInColumn.FirstOrDefault(t => t.Position == targetPos);
                if (taskAtTarget != null)
                {
                    taskAtTarget.Position = task.Position;
                    _context.Tasks.Update(taskAtTarget);
                }

                task.Position = targetPos;
            }

            task.UpdatedAt = DateTime.UtcNow;
            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }



        // ========== DELETE ==========
        public async Task DeleteTaskAsync(int taskId, int currentUserId)
        {
            var task = await GetTaskWithAccessCheckAsync(taskId, currentUserId);

            var remainingTasks = await _context.Tasks
                .Where(t => t.ColumnId == task.ColumnId && t.TaskId != task.TaskId && !t.IsArchived)
                .ToListAsync();

            ReorderTasks(remainingTasks);
            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();
        }

        // ========== ARCHIVE ==========
        public async Task<TaskDTO> ArchiveTaskAsync(int taskId, int currentUserId)
        {
            var task = await GetTaskWithAccessCheckAsync(taskId, currentUserId);
            task.IsArchived = true;
            task.UpdatedAt = DateTime.UtcNow;

            var remainingTasks = await _context.Tasks
                .Where(t => t.ColumnId == task.ColumnId && t.TaskId != task.TaskId && !t.IsArchived)
                .ToListAsync();
            ReorderTasks(remainingTasks);

            await _context.SaveChangesAsync();
            return _mapper.Map<TaskDTO>(task);
        }

        public async Task<TaskDTO> UnarchiveTaskAsync(int taskId, int currentUserId)
        {
            var task = await GetTaskWithAccessCheckAsync(taskId, currentUserId);
            task.IsArchived = false;
            task.UpdatedAt = DateTime.UtcNow;
            task.Position = await GetNextPositionAsync(task.ColumnId);

            await _context.SaveChangesAsync();
            return _mapper.Map<TaskDTO>(task);
        }

        public async Task<IEnumerable<TaskDTO>> GetArchivedTasksByBoardAsync(int boardId, int currentUserId)
        {
            var board = await _boardRepo.GetByIdAsync(boardId)
                        ?? throw new Exception("Board not found");

            if (!await HasProjectAccessAsync(currentUserId, board.ProjectId))
                throw new Exception("User has no access to this project");

            var tasks = await _repo.GetArchivedByBoardAsync(boardId);
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }
    }
}




    
