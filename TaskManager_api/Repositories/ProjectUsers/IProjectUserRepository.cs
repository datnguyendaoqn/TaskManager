using TaskManager_api.Models;

namespace TaskManager_api.Repositories.ProjectUsers
{
    public interface IProjectUserRepository
    {
        Task<ProjectUser?> GetAsync(int projectId, int userId);
        Task AddAsync(ProjectUser projectUser);
        Task RemoveAsync(ProjectUser projectUser);
        Task SaveChangesAsync();
        Task<int> CountAsync(int projectId, string? role = null);

    }

}
