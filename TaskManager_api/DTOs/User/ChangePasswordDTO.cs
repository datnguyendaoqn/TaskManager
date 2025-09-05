namespace TaskManager_api.DTOs.User
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
