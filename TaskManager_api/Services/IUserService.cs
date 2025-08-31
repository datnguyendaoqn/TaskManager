using TaskManager_api.DTOs;
using TaskManager_api.Models;

namespace TaskManager_api.Services
{
   
        public interface IUserService
        {
            Task<User?> AuthenticateAsync(UserLoginDto dto);
            Task<User> RegisterAsync(UserCreateDto dto);
            Task<IEnumerable<User>> GetAllAsync();
            Task<User?> GetByIdAsync(int id);
            Task<User?> UpdateAsync(int id, UserUpdateDto dto);
            Task<bool> DeleteAsync(int id);
        }  
}
