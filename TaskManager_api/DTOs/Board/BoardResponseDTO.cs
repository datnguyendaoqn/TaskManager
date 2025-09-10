using TaskManager_api.DTOs.BoardColumn;

namespace TaskManager_api.DTOs.Board
{
    public class BoardResponseDTO
    {
        public int BoardId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsArchived { get; set; }
        
    }
}
