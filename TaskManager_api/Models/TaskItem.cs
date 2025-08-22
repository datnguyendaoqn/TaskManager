using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;

        public int? ColumnId { get; set; }              // nullable (chỉ khi type=kanban)
        public BoardColumn? Column { get; set; }

        public int? AssignedTo { get; set; }            // nullable
        public User? AssignedToUser { get; set; }

        public int CreatedBy { get; set; }
        public User CreatedByUser { get; set; } = null!;

        [MaxLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }             // dùng khi board = list

        [MaxLength(50)]
        public string? Priority { get; set; }           // low|medium|high (optional)

        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
