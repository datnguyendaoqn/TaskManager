namespace TaskManager_api.DTOs.Task
{
    public class TaskDTO
    {
        public int TaskId { get; set; }
        public int BoardId { get; set; }
        public int ColumnId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int Position {  get; set; }
        public bool IsArchived { get; set; }
        public int? AssignedTo { get; set; }
        public string? AssignedToUserName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
