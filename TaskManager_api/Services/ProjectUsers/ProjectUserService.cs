using Microsoft.EntityFrameworkCore;
using TaskManager_api.DTOs.ProjectUser;
using TaskManager_api.Models;
using TaskManager_api.Repositories.Projects;
using TaskManager_api.Repositories.ProjectUsers;

namespace TaskManager_api.Services.ProjectUsers
{
    public class ProjectUserService : IProjectUserService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IProjectUserRepository _projectUserRepo;

        public ProjectUserService(IProjectRepository projectRepo, IProjectUserRepository projectUserRepo)
        {
            _projectRepo = projectRepo;
            _projectUserRepo = projectUserRepo;
        }

        public async Task<bool> AddUserToProjectAsync(int projectId, int currentUserId, ProjectUserAddDTO dto)
        {
            // Check project tồn tại
            var project = await _projectRepo.GetByIdAsync(projectId);
            if (project == null) return false;

            // Check currentUser có trong project và có role PM không
            var currentUserProject = await _projectUserRepo.GetAsync(projectId, currentUserId);
            if (currentUserProject == null || currentUserProject.Role != "pm") return false;

            // Check user đã tồn tại chưa
            var existing = await _projectUserRepo.GetAsync(projectId, dto.UserId);
            if (existing != null) return false;

            var newMember = new ProjectUser
            {
                ProjectId = projectId,
                UserId = dto.UserId,
                Role = "member",
                CreatedAt = DateTime.UtcNow
            };

            await _projectUserRepo.AddAsync(newMember);
            await _projectUserRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromProjectAsync(int projectId, int currentUserId, int userId)
        {
            // Check xem current user có phải PM trong project không
            var currentUserProject = await _projectUserRepo.GetAsync(projectId, currentUserId);
            if (currentUserProject == null || currentUserProject.Role != "pm")
                return false;

            // Check user cần xóa có trong project không
            var projectUser = await _projectUserRepo.GetAsync(projectId, userId);
            if (projectUser == null)
                return false;

            // Nếu user bị xóa là PM, cần check có còn PM khác không
            if (projectUser.Role == "pm")
            {
                var pmCount = await _projectUserRepo.CountAsync(projectId, role: "pm");
                if (pmCount <= 1)
                {
                    // Không cho xóa PM cuối cùng
                    return false;
                }
            }

            await _projectUserRepo.RemoveAsync(projectUser);
            await _projectUserRepo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckUserAccessAsync(int userId, int projectId)
        {
            return await _projectUserRepo.UserHasProjectAsync(userId, projectId);
        }

        public async Task<bool> IsUserPMAsync(int userId, int projectId)
        {
            var role = await _projectUserRepo.GetUserRoleInProjectAsync(userId, projectId);
            return role?.ToLower() == "pm";
        }


    }

}
