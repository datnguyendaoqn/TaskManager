namespace TaskManager_api.DTOs.Task
{
    public class TaskUpdateDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedTo { get; set; }
    }
}
