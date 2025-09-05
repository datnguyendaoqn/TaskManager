using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Auth
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId);
     
        Task SaveChangesAsync();
    }
}
