using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = " không được để trống")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "Email không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " không được để trống")]
        public string Password { get; set; } = null!;
    }
}
