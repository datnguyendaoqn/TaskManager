using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class BoardColumn
    {
        public int ColumnId { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        public int Position { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
