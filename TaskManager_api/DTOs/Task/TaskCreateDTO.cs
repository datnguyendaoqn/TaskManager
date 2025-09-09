using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.DTOs.Task
{
    public class TaskCreateDTO
    {
        [Required, MaxLength(255)]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [MaxLength(50)]
        public string? Priority { get; set; } // low|medium|high
        public DateTime? DueDate { get; set; }
    }

}
