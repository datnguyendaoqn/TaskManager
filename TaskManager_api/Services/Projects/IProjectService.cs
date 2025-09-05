using TaskManager_api.DTOs.Project;

namespace TaskManager_api.Services.Projects
{
    public interface IProjectService
    {
        Task<ProjectResponseDTO> CreateProjectAsync(ProjectCreateDTO dto, int userId);
        Task<IEnumerable<ProjectResponseDTO>> GetProjectsOfUserAsync(int userId);
        Task<ProjectResponseDTO?> GetProjectByIdAsync(int projectId, int userId);
        Task<ProjectResponseDTO?> UpdateProjectAsync(int projectId, ProjectUpdateDTO dto, int userId);
        Task<bool> DeleteProjectAsync(int projectId, int userId);
    }
}
