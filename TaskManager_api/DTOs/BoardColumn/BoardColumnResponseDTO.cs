namespace TaskManager_api.DTOs.BoardColumn
{
    public class BoardColumnResponseDTO
    {
        public int ColumnId { get; set; }
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public bool IsArchived { get; set; }
    }
}
