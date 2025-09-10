using Microsoft.AspNetCore.Mvc;
using TaskManager_api.DTOs.Auth;
using TaskManager_api.Services.Auth;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Đăng ký 
        /// </summary>
        [HttpPost("auth/register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            try
            {
                await _authService.RegisterAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                var tokens = await _authService.LoginAsync(dto);
                return Ok(new { 
                tokens.AccessToken,tokens.RefreshToken
                
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Tạo mới token JWT - dựa vào refreshtoken
        /// </summary>
        [HttpPost("auth/refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDTO dto)
        {
            try
            {
                var newAccessToken = await _authService.RefreshTokenAsync(dto.Token);
                return Ok(new { accessToken = newAccessToken });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        /// <summary>
        /// logout - xóa token JWT
        /// </summary>
        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout(RefreshTokenDTO dto)
        {
            await _authService.LogoutAsync(dto.Token);
            return Ok("Logged out");
        }
    }
}
