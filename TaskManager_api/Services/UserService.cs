using System.IdentityModel.Tokens.Jwt;
using TaskManager_api.DTOs;
using TaskManager_api.Helpers;
using TaskManager_api.Models;
using TaskManager_api.Repositories;

namespace TaskManager_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly JwtHelper _jwtHelper;

        public UserService(IUserRepository repository, JwtHelper jwtHelper)
        {
            _repository = repository;
            _jwtHelper = jwtHelper;
        }
            
        public async Task<User?> AuthenticateAsync(UserLoginDto dto)
        {
            var user = await _repository.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordHash != dto.Password) // TODO: hash check
                return null;
            return user;
        }

        public async Task<User> RegisterAsync(UserCreateDto dto)
        {
            var existing = await _repository.GetByEmailAsync(dto.Email);
            if (existing != null) throw new Exception("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.Password, // TODO: hash password
                CreatedAt = DateTime.UtcNow
            };
            return await _repository.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task<User?> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            user.FullName = dto.FullName;
            user.PasswordHash = dto.Password; // TODO: hash

            return await _repository.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);
    }
}
