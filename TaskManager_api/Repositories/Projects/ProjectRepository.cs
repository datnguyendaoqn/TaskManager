using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Projects
{
    // Repositories/ProjectRepository.cs
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task<IEnumerable<Project>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.ProjectUsers.Any(u => u.UserId == userId))
                .Include(p=>p.ProjectUsers)
                .ThenInclude(u=>u.User)
                .ToListAsync();
        }   

        public async Task<Project> AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            return project;
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
