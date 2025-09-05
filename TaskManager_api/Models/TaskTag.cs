using System.Text.Json.Serialization;

namespace TaskManager_api.Models
{
    // Join entity (payload: created_at, updated_at)
    public class TaskTag
    {
        public int TaskId { get; set; }
        [JsonIgnore]

        public TaskItem Task { get; set; } = null!;

        public int TagId { get; set; }
        [JsonIgnore]

        public Tag Tag { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
