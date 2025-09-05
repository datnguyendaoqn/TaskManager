using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using TaskManager_api.DTOs.Auth;
using TaskManager_api.DTOs.User;
using TaskManager_api.Helpers;
using TaskManager_api.Models;
using TaskManager_api.Repositories.Users;

namespace TaskManager_api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;


        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public static bool VerifyPassword(string Password, string PasswordHash)
        {
            // Check password bằng BCrypt
            bool isPasswordVerify = BCrypt.Net.BCrypt.Verify(Password, PasswordHash);
            if (!isPasswordVerify) 
                return false;
            return true;
        }

        public async Task<UserProfileDTO?> GetProfileAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null) return null;
            return _mapper.Map<UserProfileDTO>(user);
        }
        public async Task UpdateProfileAsync(int userId, UserUpdateDto dto)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");
            _mapper.Map(dto,user);
            await _repository.UpdateAsync(user);          
        }
        public async Task ChangePasswordAsync(int userId, ChangePasswordDTO dto)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var verify = VerifyPassword(dto.OldPassword, user.PasswordHash);
            if (!verify)
                throw new Exception("Old password is incorrect");
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordHash = passwordHash;
            user.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(user);
         
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);
    }
}
