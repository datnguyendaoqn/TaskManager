using System.ComponentModel.DataAnnotations;

namespace TaskManager_api.DTOs.BoardColumn
{
    public class BoardColumnCreateDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        public int Position { get; set; }
    }
}
