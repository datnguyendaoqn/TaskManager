using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Repositories.BoardColumns
{
    public class BoardColumnRepository : IBoardColumnRepository
    {
        private readonly AppDbContext _context;

        public BoardColumnRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BoardColumn column)
        {
            await _context.BoardColumns.AddAsync(column);
        }

        public async Task<BoardColumn?> GetByIdAsync(int columnId, bool includeArchived = false)
        {
            var query = _context.BoardColumns.AsQueryable();
            if (!includeArchived)
                query = query.Where(c => !c.IsArchived);

            return await query.FirstOrDefaultAsync(c => c.ColumnId == columnId);
        }

        public async Task RemoveAsync(BoardColumn column)
        {
            _context.BoardColumns.Remove(column);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
