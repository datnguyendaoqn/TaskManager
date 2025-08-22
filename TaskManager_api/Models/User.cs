using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class User
    {
        public int UserId { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; } = null!;

        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<TaskItem> TasksAssigned { get; set; } = new List<TaskItem>();
        public ICollection<TaskItem> TasksCreated { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
