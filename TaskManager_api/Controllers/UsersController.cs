using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.Auth;
using TaskManager_api.DTOs.User;
using TaskManager_api.Helpers;
using TaskManager_api.Models;
using TaskManager_api.Services.Users;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtHelper _jwtHelper;

        public UserController(IUserService service, JwtHelper jwtHelper)
        {
            _service = service;
            _jwtHelper = jwtHelper;
        }
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _service.GetProfileAsync(id);
            if (profile == null) return NotFound();

            return Ok(profile);
        }
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UserUpdateDto dto)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _service.UpdateProfileAsync(id,dto);
            return NoContent();
        }
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _service.ChangePasswordAsync(id, dto);
            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}