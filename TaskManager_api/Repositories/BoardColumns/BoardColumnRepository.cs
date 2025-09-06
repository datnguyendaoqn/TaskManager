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

        public async Task<int> GetMaxPositionAsync(int boardId)
        {
            return await _context.BoardColumns
                .Where(c => c.BoardId == boardId && !c.IsArchived)
                .MaxAsync(c => (int?)c.Position) ?? 0;
        }

        public async Task<BoardColumn?> GetByIdAsync(int columnId, bool includeArchived = false)
        {
            var query = _context.BoardColumns.AsQueryable();
            if (!includeArchived)
                query = query.Where(c => !c.IsArchived);

            return await query.Include(b=>b.Board).FirstOrDefaultAsync(c => c.ColumnId == columnId);
        }

        public async Task<IEnumerable<BoardColumn>> GetByBoardIdAsync(int boardId, bool includeArchived = false)
        {
            var query = _context.BoardColumns.AsQueryable();
            query = query.Where(c => c.BoardId == boardId);
            if (!includeArchived)
                query = query.Where(c => !c.IsArchived);

            return await query.OrderBy(c => c.Position).ToListAsync();
        }

        public async Task<IEnumerable<BoardColumn>> GetArchivedByBoardIdAsync(int boardId)
        {
            return await _context.BoardColumns
                .Where(c => c.BoardId == boardId && c.IsArchived)
                .OrderBy(c => c.Position)
                .ToListAsync();
        }
        public async Task<BoardColumn?> GetByBoardIdAndPositionAsync(int boardId, int position)
        {
            return await _context.BoardColumns
                .FirstOrDefaultAsync(c => c.BoardId == boardId && c.Position == position && !c.IsArchived);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Thêm hàm update để service dễ thao tác với entity
        public void Update(BoardColumn column)
        {
            _context.BoardColumns.Update(column);
        }

        public async Task RemoveAsync(BoardColumn column)
        {
            _context.BoardColumns.Remove(column);
        }
    }

}
