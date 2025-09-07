using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.BoardColumn;
using TaskManager_api.Services.BoardColumns;

namespace TaskManager_api.Controllers
{
    [ApiController]
[Route("api/boards/{boardId}/columns")]
[Authorize]
public class BoardColumnsController : ControllerBase
{
        private readonly IBoardColumnService _columnService;

        public BoardColumnsController(IBoardColumnService columnService)
        {
            _columnService = columnService;
        }
        /// <summary>
        /// lấy các column trong 1 board
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetColumn(int boardId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var columns = await _columnService.GetColumnAsync(boardId,currentUserId);
            return Ok(columns);
        }
        /// <summary>
        /// Tạo column mới trong 1 board
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddColumn(int boardId, [FromBody] BoardColumnCreateDTO dto)
        {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var column = await _columnService.AddColumnAsync(boardId, dto,currentUserId);
                 return Ok(column);
        }
        /// <summary>
        /// Cập nhật column
        /// </summary>
            [HttpPatch("{columnId}")]
            public async Task<IActionResult> UpdateColumn(int columnId, [FromBody] BoardColumnCreateDTO dto)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var success = await _columnService.UpdateColumnAsync(columnId, dto, currentUserId);
                if (!success) return NotFound();
                return NoContent();
            }
        /// <summary>
        /// Move column
        /// </summary>
        [HttpPatch("{columnId}/move")]
        public async Task<IActionResult> MoveColumn(int columnId,[FromBody] BoardColumnMoveDTO dto)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _columnService.MoveColumnAsync(columnId, dto.position, currentUserId);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>       
        /// Archive column (ẩn đi, không xóa hẳn)
        /// </summary>
        [HttpPatch("{columnId}/archive")]
    public async Task<IActionResult> ArchiveColumn(int columnId)
    {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _columnService.ArchiveColumnAsync(columnId, currentUserId);
            if (!success) return NotFound();
            return NoContent();
    }
        /// <summary>
        /// Unarchive column (khôi phục lại)
        /// </summary>
        [HttpPatch("{columnId}/unarchive")]
    public async Task<IActionResult> UnarchiveColumn(int columnId)
    {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _columnService.UnarchiveColumnAsync(columnId, currentUserId);
        if (!success) return NotFound();
        return NoContent();
    }
        /// <summary>
        /// Xóa hẳn column (delete permanently)       
        /// </summary>
        [HttpDelete("{columnId}")]
    public async Task<IActionResult> DeleteColumnPermanently(int columnId)
    {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _columnService.DeleteColumnPermanentlyAsync(columnId, currentUserId);
        if (!success) return NotFound();
        return NoContent();
    }
        /// <summary>
        /// Lấy thông tin các column đã archived
        /// </summary>
       
        [HttpGet("archived")]
    public async Task<IActionResult> GetArchivedColumns(int boardId)
    {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var columns = await _columnService.GetArchivedColumnsAsync(boardId, currentUserId);
             return Ok(columns);
    }
}
}
