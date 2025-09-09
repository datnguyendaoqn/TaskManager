using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Boards
{
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _context;

        public BoardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
        }

        public async Task<Board?> GetByIdAsync(int boardId, bool includeArchived = false)
        {
            var query = _context.Boards.Include(b => b.Columns).AsQueryable();
            if (!includeArchived) query = query.Where(b => !b.IsArchived);
            return await query.FirstOrDefaultAsync(b => b.BoardId == boardId);
        }

        public async Task<IEnumerable<Board>> GetByProjectIdAsync(int projectId, bool includeArchived = false)
        {
            var query = _context.Boards
                .Include(b => b.Columns)
                .Where(b => b.ProjectId == projectId);
            if (!includeArchived) query = query.Where(b => !b.IsArchived);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Board>> GetArchivedByProjectIdAsync(int projectId) 
        {
            return await _context.Boards
                .Include(b => b.Columns)
                .Where(b => b.ProjectId == projectId && b.IsArchived)
                .ToListAsync();
        }
        public async Task RemoveAsync(Board board)
        {
            _context.Boards.Remove(board);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
