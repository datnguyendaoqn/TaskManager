using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Auth
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }
        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
