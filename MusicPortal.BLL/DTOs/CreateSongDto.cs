using System.ComponentModel.DataAnnotations;

namespace MusicPortal.BLL.DTOs
{
    /// <summary>
    /// DTO для створення нової пісні
    /// </summary>
    public class CreateSongDto
    {
        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(100, ErrorMessage = "TitleLength")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "ArtistRequired")]
        [StringLength(100, ErrorMessage = "ArtistLength")]
        public string? Artist { get; set; }

        [Required]
        public string? FileName { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
