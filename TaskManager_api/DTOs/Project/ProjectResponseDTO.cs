namespace TaskManager_api.DTOs.Project
{
    public class ProjectResponseDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProjectMemberDTO> Members { get; set; } = new();
    }
    public class ProjectMemberDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
