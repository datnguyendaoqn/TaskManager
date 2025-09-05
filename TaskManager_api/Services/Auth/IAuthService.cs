using TaskManager_api.DTOs.Auth;

namespace TaskManager_api.Services.Auth
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> LoginAsync(LoginDTO dto);
        Task<string> RefreshTokenAsync(string refreshToken);
        Task RegisterAsync(RegisterDTO dto);
        Task LogoutAsync(string refreshToken);
    }
}
