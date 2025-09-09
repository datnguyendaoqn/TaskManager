using TaskManager_api.DTOs.ProjectUser;

namespace TaskManager_api.Services.ProjectUsers
{
    public interface IProjectUserService
    {
        Task<bool> AddUserToProjectAsync(int projectId, int currentUserId, ProjectUserAddDTO dto);
        Task<bool> RemoveUserFromProjectAsync(int projectId, int currentUserId, int userId);
        Task<bool> CheckUserAccessAsync(int userId, int projectId);
        Task<bool> IsUserPMAsync(int userId, int projectId);



    }

}
