using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Repositories.ProjectUsers
{
    public class ProjectUserRepository : IProjectUserRepository
    {
        private readonly AppDbContext _context;

        public ProjectUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectUser?> GetAsync(int projectId, int userId)
        {
            return await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
        }

        public async Task AddAsync(ProjectUser projectUser)
        {
            await _context.ProjectUsers.AddAsync(projectUser);
        }

        public async Task RemoveAsync(ProjectUser projectUser)
        {
            _context.ProjectUsers.Remove(projectUser);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<int> CountAsync(int projectId, string? role = null)
        {
            var query = _context.ProjectUsers.Where(pu => pu.ProjectId == projectId);
            if (!string.IsNullOrEmpty(role))
                query = query.Where(pu => pu.Role == role);

            return await query.CountAsync();
        }
        public async Task<bool> UserHasProjectAsync(int userId, int projectId)
        {
            return await _context.ProjectUsers
                .AnyAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
        }

        public async Task<string?> GetUserRoleInProjectAsync(int userId, int projectId)
        {
            var pu = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
            return pu?.Role;
        }

    }

}
