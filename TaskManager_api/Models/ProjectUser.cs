using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManager_api.Models
{
    // Join entity (payload: role, joined_at, created/updated)
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; } = null!;
        public int UserId { get; set; }
        [JsonIgnore]

        public User User { get; set; } = null!;

        [MaxLength(50)]
        public string Role { get; set; } = null!; // 'pm' | 'member'

        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
