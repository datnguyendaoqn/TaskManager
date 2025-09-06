using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.Task;
using TaskManager_api.Services.Tasks;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tạo task trong 1 column
        /// </summary>
        [HttpPost("columns/{columnId}/tasks")]
        public async Task<IActionResult> Create(int columnId, TaskCreateDTO dto)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var task = await _service.CreateTaskAsync(columnId, currentUserId, dto);
            return CreatedAtAction(nameof(Get), new { taskId = task.TaskId }, task);
        }

        /// <summary>
        /// Lấy thông tin 1 task
        /// </summary>
        [HttpGet("tasks/{taskId}")]
        public async Task<IActionResult> Get(int taskId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var task = await _service.GetTaskAsync(taskId, currentUserId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        /// <summary>
        /// Lấy toàn bộ task trong 1 board
        /// </summary>
        [HttpGet("boards/{boardId}/tasks")]
        public async Task<IActionResult> GetByBoard(int boardId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tasks = await _service.GetTasksByBoardAsync(boardId, currentUserId);
            return Ok(tasks);
        }

        /// <summary>
        /// Lấy toàn bộ task trong 1 column
        /// </summary>
        [HttpGet("columns/{columnId}/tasks")]
        public async Task<IActionResult> GetByColumn(int columnId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tasks = await _service.GetTasksByColumnAsync(columnId, currentUserId);
            return Ok(tasks);
        }

        /// <summary>
        /// Cập nhật thông tin task (title, desc, assigned…)
        /// </summary>
        [HttpPatch("tasks/{taskId}")]
        public async Task<IActionResult> UpdateInfo(int taskId, TaskUpdateDTO dto)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var updated = await _service.UpdateTaskInfoAsync(taskId, currentUserId, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Di chuyển hoặc thay đổi vị trí task
        /// </summary>
        [HttpPatch("tasks/{taskId}/move")]
        public async Task<IActionResult> Move(int taskId, [FromBody] MoveTaskDTO dto)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var moved = await _service.MoveTaskAsync(taskId, currentUserId, dto);
            return Ok(moved);
        }

        /// <summary>
        /// Xóa task
        /// </summary>
        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _service.DeleteTaskAsync(taskId, currentUserId);
            return NoContent();
        }

        /// <summary>
        /// Archive task
        /// </summary>
        [HttpPatch("tasks/{taskId}/archive")]
        public async Task<IActionResult> Archive(int taskId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var task = await _service.ArchiveTaskAsync(taskId, currentUserId);
            return Ok(task);
        }

        /// <summary>
        /// Unarchive task
        /// </summary>
        [HttpPatch("tasks/{taskId}/unarchive")]
        public async Task<IActionResult> Unarchive(int taskId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var task = await _service.UnarchiveTaskAsync(taskId, currentUserId);
            return Ok(task);
        }

        /// <summary>
        /// Lấy thông tin các task archived trong board
        /// </summary>
        [HttpGet("boards/{boardId}/tasks/archived")]
        public async Task<IActionResult> GetArchivedByBoard(int boardId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tasks = await _service.GetArchivedTasksByBoardAsync(boardId, currentUserId);
            return Ok(tasks);
        }
    }



}
