namespace TaskManager_api.DTOs.Task
{
    public class MoveTaskDTO
    {
        /// <summary>
        /// ID của column mới (nếu muốn di chuyển sang column khác). 
        /// Nếu chỉ đổi vị trí trong column cũ thì để null.
        /// </summary>
        public int? NewColumnId { get; set; }

        /// <summary>
        /// Vị trí mới trong column (nếu muốn reorder).
        /// Nếu chỉ đổi column mà không quan tâm vị trí thì để null.
        /// </summary>
        public int? NewPosition { get; set; }
    }
}
