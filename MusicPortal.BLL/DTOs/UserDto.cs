namespace MusicPortal.BLL.DTOs
{
    /// <summary>
    /// DTO для передачі даних користувача між шарами
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SongsCount { get; set; }
    }
}
