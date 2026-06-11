namespace MusicPortal.BLL.DTOs
{
    /// <summary>
    /// DTO для передачі даних жанру між шарами
    /// </summary>
    public class GenreDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SongsCount { get; set; }
    }
}
