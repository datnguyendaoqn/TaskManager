namespace TaskManager_api.DTOs.Auth
{
    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string DeviceId { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
    }
    public class RefreshTokenDTO123
    {
        public string Token { get; set; }
    }
}
