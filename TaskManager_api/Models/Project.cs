using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int CreatedBy { get; set; }         // FK -> user.user_id
        public User CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<Board> Boards { get; set; } = new List<Board>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
