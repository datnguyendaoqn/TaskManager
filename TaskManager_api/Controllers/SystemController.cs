using Microsoft.AspNetCore.Mvc;
using TaskManager_api.Data;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SystemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("memory-stats")]
        public IActionResult GetMemoryStats()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            var recordCounts = new
            {
                Users = _context.Users.Count(),
                Projects = _context.Projects.Count(),
                Tasks = _context.Tasks.Count(),
                Boards = _context.Boards.Count(),
                BoardColumns = _context.BoardColumns.Count()
            };

            return Ok(new
            {
                ProcessMemoryMB = process.WorkingSet64 / (1024 * 1024),
                PrivateMemoryMB = process.PrivateMemorySize64 / (1024 * 1024),
                RecordCounts = recordCounts,
                GCTotalMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024)
            });
        }
    }
}
