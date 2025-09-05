using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManager_api.DTOs;
using TaskManager_api.DTOs.Auth;
using TaskManager_api.DTOs.User;
using TaskManager_api.Models;

namespace TaskManager_api.Services.Users
{
   
        public interface IUserService
        {          
        Task<UserProfileDTO?> GetProfileAsync(int userId);
        Task UpdateProfileAsync(int userId, UserUpdateDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDTO dto);
            Task<bool> DeleteAsync(int id);
        }  
}
