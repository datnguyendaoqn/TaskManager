using TaskManager_api.Models;

namespace TaskManager_api.Repositories.Projects
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllByUserIdAsync(int userId);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task SaveChangesAsync();
    }
}
