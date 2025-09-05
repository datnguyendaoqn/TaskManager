using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]

        [MaxLength(50)]
        public string SystemRole { get; set; } = "User"; // Admin, User
        [MaxLength(500)]
        public string? Bio { get; set; }   // mô tả ngắn về user, có thể null

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }  // link đến ảnh đại diện, có thể null
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        [JsonIgnore]

        public ICollection<TaskItem> TasksAssigned { get; set; } = new List<TaskItem>();
        [JsonIgnore]

        public ICollection<TaskItem> TasksCreated { get; set; } = new List<TaskItem>();
        [JsonIgnore]

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [JsonIgnore]

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        [JsonIgnore]
        
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
