using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManager_api.Models
{
    public class BoardColumn
    {
        public int ColumnId { get; set; }

        public int BoardId { get; set; }
        [JsonIgnore]

        public Board Board { get; set; } = null!;

        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public bool IsArchived { get; set; } = false;
        public int Position { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [JsonIgnore]

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
