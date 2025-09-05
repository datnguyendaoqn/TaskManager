using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.DTOs.Board
{
    public class BoardCreateDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
