using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.ProjectUser;
using TaskManager_api.Services.ProjectUsers;

namespace TaskManager_api.Controllers
{
    //[ApiController]
    //[Route("api/project/{projectId}/user")]
    //[Authorize]
    //public class ProjectUsersController : ControllerBase
    //{
    //    private readonly IProjectUserService _projectUserService;

    //    public ProjectUsersController(IProjectUserService projectUserService)
    //    {
    //        _projectUserService = projectUserService;
    //    }
    //    /// <summary>
    //    /// Thêm user vào project
    //    /// </summary>
    //    [HttpPost]
    //    public async Task<IActionResult> AddUser(int projectId, [FromBody] ProjectUserAddDTO dto)
    //    {
    //        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    //        var success = await _projectUserService.AddUserToProjectAsync(projectId, currentUserId, dto);
    //        if (!success) return Forbid();
    //        return Ok();
    //    }
    //    /// <summary>
    //    /// Xóa user khỏi project
    //    /// </summary>
    //    [HttpDelete("{userId}")]
    //    public async Task<IActionResult> RemoveUser(int projectId, int userId)
    //    {
    //        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    //        var success = await _projectUserService.RemoveUserFromProjectAsync(projectId, currentUserId, userId);
    //        if (!success) return Forbid();
    //        return NoContent();
    //    }
    //}

}
