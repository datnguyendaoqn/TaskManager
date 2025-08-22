using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class Board
    {
        public int BoardId { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string Type { get; set; } = null!; // 'kanban' | 'list'

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
