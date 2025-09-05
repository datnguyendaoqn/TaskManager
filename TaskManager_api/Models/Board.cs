using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManager_api.Models
{
    public class Board
    {
        public int BoardId { get; set; }

        public int ProjectId { get; set; }
        [JsonIgnore]

        public Project Project { get; set; } = null!;

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        public bool IsArchived { get; set; } = false;


        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [JsonIgnore]

        public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();
        [JsonIgnore]

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
