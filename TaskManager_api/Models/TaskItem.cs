using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManager_api.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }

        public int BoardId { get; set; }
        [JsonIgnore]

        public Board Board { get; set; } = null!;

        public int ColumnId { get; set; }             
        [JsonIgnore]

        public BoardColumn? Column { get; set; }
        [JsonIgnore]

        public int? AssignedTo { get; set; }
        [JsonIgnore]
                       
        public User? AssignedToUser { get; set; }
        [JsonIgnore]


        public int CreatedBy { get; set; }
        [JsonIgnore]

        public User CreatedByUser { get; set; } = null!;

        [MaxLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
        [MaxLength(50)]
        public string? Priority { get; set; }           // low|medium|high (optional)
        public bool IsArchived { get; set; } = false;
        public int Position { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [JsonIgnore]

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        [JsonIgnore]

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
