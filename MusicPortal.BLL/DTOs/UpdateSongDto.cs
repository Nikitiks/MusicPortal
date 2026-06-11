using System.ComponentModel.DataAnnotations;

namespace MusicPortal.BLL.DTOs
{
    /// <summary>
    /// DTO для оновлення існуючої пісні
    /// </summary>
    public class UpdateSongDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(100, ErrorMessage = "TitleLength")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "ArtistRequired")]
        [StringLength(100, ErrorMessage = "ArtistLength")]
        public string? Artist { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}
