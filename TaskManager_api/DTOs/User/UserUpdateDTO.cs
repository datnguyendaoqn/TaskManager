namespace TaskManager_api.DTOs.User
{
    public class UserUpdateDto
    {
        public string? FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
    }
}
