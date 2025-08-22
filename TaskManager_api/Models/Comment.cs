namespace TaskManager_api.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
