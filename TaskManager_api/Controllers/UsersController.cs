using Microsoft.AspNetCore.Mvc;
using TaskManager_api.DTOs;
using TaskManager_api.Helpers;
using TaskManager_api.Models;
using TaskManager_api.Services;

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

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserCreateDto dto)
        {
            try
            {
                var user = await _service.RegisterAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto dto)
        {
            var user = await _service.AuthenticateAsync(dto);
            if (user == null) return Unauthorized();

            // Tạm hardcode role = "User" (sau có thể lấy từ DB)
            var token = _jwtHelper.GenerateToken(user, "User");
            return Ok(new { token });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, UserUpdateDto dto)
        {
            var user = await _service.UpdateAsync(id, dto);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}