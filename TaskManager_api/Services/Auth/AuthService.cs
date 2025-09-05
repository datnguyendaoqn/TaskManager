using AutoMapper;
using System.Security.Cryptography;
using TaskManager_api.DTOs.Auth;
using TaskManager_api.Helpers;
using TaskManager_api.Models;
using TaskManager_api.Repositories.Auth;
using TaskManager_api.Repositories.Users;

namespace TaskManager_api.Services.Auth
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly JwtHelper _jwtHelper;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepo, IRefreshTokenRepository refreshRepo, JwtHelper jwtHelper, IMapper mapper)
        {
            _userRepo = userRepo;
            _refreshRepo = refreshRepo;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email);
            if (existing != null) throw new Exception("Email already exists");
            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _userRepo.AddAsync(user);
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Wrong password!!");

            // Revoke token cũ của user trên device khác
            var oldTokens = await _refreshRepo.GetByUserIdAsync(user.UserId);
          
            foreach (var t in oldTokens)
            {
                if (t.Revoked == null && t.DeviceId != dto.DeviceId)
                    t.Revoked = DateTime.UtcNow;
            }
            await _refreshRepo.SaveChangesAsync();
            

            // Tạo token mới
            var accessToken = _jwtHelper.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            await _refreshRepo.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId,
                Expires = DateTime.UtcNow.AddDays(7),
                DeviceId = dto.DeviceId,
                DeviceName = dto.DeviceName
            });
            await _refreshRepo.SaveChangesAsync();

            return(accessToken, refreshToken);
                          
            
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (token == null || token.Revoked != null || token.Expires < DateTime.UtcNow)
                throw new Exception("Refresh token invalid");

            var user = await _userRepo.GetByIdAsync(token.UserId);
            if (user == null) throw new Exception("User not found");

            // Tạo access token mới
            return _jwtHelper.GenerateToken(user);
        }

        public async Task LogoutAsync(string token)
        {
            var rt = await _refreshRepo.GetByTokenAsync(token);
            if (rt == null) throw new Exception("Refresh token invalid");

            rt.Revoked = DateTime.UtcNow;
            await _refreshRepo.SaveChangesAsync();
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
