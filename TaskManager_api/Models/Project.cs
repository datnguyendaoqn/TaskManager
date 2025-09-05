using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManager_api.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }         // FK -> user.user_id
        [JsonIgnore]

        public User CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [JsonIgnore]

        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        [JsonIgnore]

        public ICollection<Board> Boards { get; set; } = new List<Board>();
        [JsonIgnore]

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
