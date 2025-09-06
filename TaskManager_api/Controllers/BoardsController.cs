using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_api.DTOs.Board;
using TaskManager_api.Services.Boards;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/boards")]
    [Authorize]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        /// <summary>
        /// Tạo board mới (auto 3 column mặc định: To Do, In Progress, Done)
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> CreateBoard(int projectId, [FromBody] BoardCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var board = await _boardService.CreateBoardAsync(projectId, userId, dto);
            return Ok(board);
        }
        ///<summary>
        /// Lấy chi tiết 1 board (kèm danh sách column)
        ///</summary>

        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetBoardDetail(int boardId, [FromQuery] bool includeArchived = false)
        {
            var board = await _boardService.GetBoardDetailAsync(boardId, includeArchived);
            if (board == null) return NotFound();
            return Ok(board);
        }
        /// <summary>
        /// Lấy thông tin các board đã archive
        /// </summary>
        
        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedBoards(int projectId)
        {
            var boards = await _boardService.GetArchivedBoardsAsync(projectId);
            return Ok(boards);
        }
        /// <summary>
        /// Lấy danh sách board trong project
        
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBoardsOfProject(int projectId, [FromQuery] bool includeArchived = false)
        {
            var boards = await _boardService.GetBoardsOfProjectAsync(projectId, includeArchived);
            return Ok(boards);
        }
        /// <summary>
        /// Archive board
        
        /// </summary>
        [HttpPatch("{boardId}/archive")]
        public async Task<IActionResult> ArchiveBoard(int boardId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _boardService.ArchiveBoardAsync(boardId, userId);
            if (!success) return NotFound();
            return NoContent();
        }
        /// <summary>
        /// Unarchive board
        
        /// </summary>
        [HttpPatch("{boardId}/unarchive")]
        public async Task<IActionResult> UnarchiveBoard(int boardId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _boardService.UnarchiveBoardAsync(boardId, userId);
            if (!success) return NotFound();
            return NoContent();
        }
        /// <summary>
        /// Delete permanently (chỉ khi đã archived thì FE mới cho gọi)
        
        /// </summary>
        [HttpDelete("{boardId}")]
        public async Task<IActionResult> DeleteBoardPermanently(int boardId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _boardService.DeleteBoardPermanentlyAsync(boardId, userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }


}