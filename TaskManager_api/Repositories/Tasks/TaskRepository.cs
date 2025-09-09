using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // Domain queries (giữ lại)
        public async Task<TaskItem?> GetByIdWithProjectCheckAsync(int taskId, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Column)
                .ThenInclude(c => c.Board)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null) return null;

            var hasAccess = await _context.ProjectUsers
                .AnyAsync(pu => pu.ProjectId == task.Board.ProjectId && pu.UserId == userId);

            return hasAccess ? task : null;
        }

        public async Task<IEnumerable<TaskItem>> GetByBoardAsync(int boardId, bool includeArchived = false)
        {
            var query = _context.Tasks.AsQueryable();
            if (!includeArchived) query = query.Where(t => !t.IsArchived);

            return await query
                .Where(t => t.BoardId == boardId)
                .Include(t => t.Board)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByColumnAsync(int columnId, bool includeArchived = false)
        {
            var query = _context.Tasks.AsQueryable();
            if (!includeArchived) query = query.Where(t => !t.IsArchived);

            return await query
                .Where(t => t.ColumnId == columnId)
                .Include(t => t.Board)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetArchivedByBoardAsync(int boardId)
        {
            return await _context.Tasks
                .Where(t => t.BoardId == boardId && t.IsArchived)
                .Include(t => t.Board)
                .ToListAsync();
        }
    }

}
