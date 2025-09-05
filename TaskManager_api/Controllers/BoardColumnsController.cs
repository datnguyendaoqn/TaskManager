using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    // Tạo column mới trong 1 board
    [HttpPost]
    public async Task<IActionResult> AddColumn(int boardId, [FromBody] BoardColumnCreateDTO dto)
    {
        var column = await _columnService.AddColumnAsync(boardId, dto);
        return Ok(column);
    }

    // Cập nhật column
    [HttpPut("{columnId}")]
    public async Task<IActionResult> UpdateColumn(int columnId, [FromBody] BoardColumnCreateDTO dto)
    {
        var success = await _columnService.UpdateColumnAsync(columnId, dto);
        if (!success) return NotFound();
        return NoContent();
    }

    // Archive column (ẩn đi, không xóa hẳn)
    [HttpPatch("{columnId}/archive")]
    public async Task<IActionResult> ArchiveColumn(int columnId)
    {
        var success = await _columnService.ArchiveColumnAsync(columnId);
        if (!success) return NotFound();
        return NoContent();
    }

    // Unarchive column (khôi phục lại)
    [HttpPatch("{columnId}/unarchive")]
    public async Task<IActionResult> UnarchiveColumn(int columnId)
    {
        var success = await _columnService.UnarchiveColumnAsync(columnId);
        if (!success) return NotFound();
        return NoContent();
    }

    // Xóa hẳn column (delete permanently)
    [HttpDelete("{columnId}")]
    public async Task<IActionResult> DeleteColumnPermanently(int columnId)
    {
        var success = await _columnService.DeleteColumnPermanentlyAsync(columnId);
        if (!success) return NotFound();
        return NoContent();
    }
}


}
