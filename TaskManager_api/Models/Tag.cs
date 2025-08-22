using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Color { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
