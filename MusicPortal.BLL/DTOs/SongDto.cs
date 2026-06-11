namespace MusicPortal.BLL.DTOs
{
    /// <summary>
    /// DTO для передачі даних пісні між шарами
    /// </summary>
    public class SongDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public int GenreId { get; set; }
        public string? GenreName { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
    }
}
