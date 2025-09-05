using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.Project;
using TaskManager_api.Services.Projects;

namespace TaskManager_api.Controllers
{
    // Controllers/ProjectController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }
        /// <summary>
        /// Tạo mới 1 project
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]ProjectCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.CreateProjectAsync(dto, userId);
            return Ok(result);
        }
        /// <summary>
        /// Lấy danh sách project của user hiện tại.
        /// </summary>
        /// <response code="200">Danh sách project</response>
        /// <response code="401">Chưa đăng nhập hoặc token sai</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserProjects()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var projects = await _service.GetProjectsOfUserAsync(userId);
            return Ok(projects);
        }
        /// <summary>
        /// Lấy thông tin của 1 project - những người tham gia project 
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var project = await _service.GetProjectByIdAsync(id, userId);
            if (project == null) return Forbid();
            return Ok(project);
        }
        /// <summary>
        /// Cập nhật thông tin project
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id,[FromBody]ProjectUpdateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.UpdateProjectAsync(id, dto, userId);
            if (result == null) return Forbid();
            return Ok(result);
        }
        /// <summary>
        /// Xóa project
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _service.DeleteProjectAsync(id, userId);
            if (!success) return Forbid();
            return NoContent();
        }
    }

}
