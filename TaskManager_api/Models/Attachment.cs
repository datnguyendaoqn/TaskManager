using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }

        public int TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [MaxLength(500)]
        public string FilePath { get; set; } = null!;

        [MaxLength(50)]
        public string FileType { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
