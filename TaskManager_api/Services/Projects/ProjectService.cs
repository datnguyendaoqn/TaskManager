using AutoMapper;
using TaskManager_api.DTOs.Project;
using TaskManager_api.Models;
using TaskManager_api.Repositories.Projects;

namespace TaskManager_api.Services.Projects
{
    // Services/ProjectService.cs
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDTO> CreateProjectAsync(ProjectCreateDTO dto, int userId)
        {       
            var project = _mapper.Map<Project>(dto);
            project.CreatedBy = userId;
            project.CreatedAt = DateTime.Now;
            // Add default ProjectUser as PM
            project.ProjectUsers.Add(new ProjectUser
            {
                UserId = userId,
                Role = "pm",
                CreatedAt = DateTime.UtcNow
            });

            await _repo.AddAsync(project);
            await _repo.SaveChangesAsync();

            return _mapper.Map<ProjectResponseDTO>(project);
        }

        public async Task<IEnumerable<ProjectResponseDTO>> GetProjectsOfUserAsync(int userId)
        {
            var projects = await _repo.GetAllByUserIdAsync(userId);
            return projects.Select(MapToResponse);
        }

        public async Task<ProjectResponseDTO?> GetProjectByIdAsync(int projectId, int userId)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) return null;

            var isMember = project.ProjectUsers.Any(u => u.UserId == userId);
            if (!isMember) return null; // Forbidden

            return MapToResponse(project);
        }

        public async Task<ProjectResponseDTO?> UpdateProjectAsync(int projectId, ProjectUpdateDTO dto, int userId)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) return null;

            if (project.CreatedBy != userId) return null; // Only creator (PM) can update

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(project);
            await _repo.SaveChangesAsync();

            return MapToResponse(project);
        }

        public async Task<bool> DeleteProjectAsync(int projectId, int userId)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) return false;

            if (project.CreatedBy != userId) return false; // Only creator can delete

            await _repo.DeleteAsync(project);
            await _repo.SaveChangesAsync();
            return true;
        }

        private ProjectResponseDTO MapToResponse(Project project) =>
            new ProjectResponseDTO
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Description = project.Description,
                CreatedBy = project.CreatedBy,
                CreatedAt = project.CreatedAt,
                Members = project.ProjectUsers.Select(pu => new ProjectMemberDTO
                 {
                     UserId = pu.UserId,
                     UserName = pu.User.FullName, // nhớ Include User khi query
                     Role = pu.Role
                 }).ToList()
            };
    }

}
