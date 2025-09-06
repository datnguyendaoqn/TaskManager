using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.Project;
using TaskManager_api.DTOs.ProjectUser;
using TaskManager_api.Services.Projects;
using TaskManager_api.Services.ProjectUsers;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IProjectUserService _projectUserService;

        public ProjectsController(IProjectService service, IProjectUserService projectUserService)
        {
            _service = service;
            _projectUserService = projectUserService;
        }

        /// <summary>
        /// Tạo mới project
        /// </summary>
        /// <param name="dto">Dữ liệu project</param>
        /// <returns>Project vừa tạo</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.CreateProjectAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { projectId = result.ProjectId }, result);
        }

        /// <summary>
        /// Lấy danh sách project của user hiện tại
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserProjects()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var projects = await _service.GetProjectsOfUserAsync(userId);
            return Ok(projects);
        }

        /// <summary>
        /// Lấy thông tin project theo ID
        /// </summary>
        [HttpGet("{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int projectId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var project = await _service.GetProjectByIdAsync(projectId, userId);
            if (project == null) return Forbid();
            return Ok(project);
        }

        /// <summary>
        /// Cập nhật thông tin project (partial update)
        /// </summary>
        [HttpPatch("{projectId}")]
        [Authorize]
        public async Task<IActionResult> Update(int projectId, [FromBody] ProjectUpdateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.UpdateProjectAsync(projectId, dto, userId);
            if (result == null) return Forbid();
            return Ok(result);
        }

        /// <summary>
        /// Xóa project
        /// </summary>
        [HttpDelete("{projectId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int projectId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _service.DeleteProjectAsync(projectId, userId);
            if (!success) return Forbid();
            return NoContent();
        }

        /// <summary>
        /// Thêm user vào project
        /// </summary>
        [HttpPost("{projectId}/users")]
        [Authorize]
        public async Task<IActionResult> AddUser(int projectId, [FromBody] ProjectUserAddDTO dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _projectUserService.AddUserToProjectAsync(projectId, currentUserId, dto);
            if (!success) return Forbid();
            return Ok();
        }

        /// <summary>
        /// Xóa user khỏi project
        /// </summary>
        [HttpDelete("{projectId}/users/{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveUser(int projectId, int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _projectUserService.RemoveUserFromProjectAsync(projectId, currentUserId, userId);
            if (!success) return Forbid();
            return NoContent();
        }
    }


}
