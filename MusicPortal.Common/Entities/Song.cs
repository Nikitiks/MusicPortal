using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Common.Entities
{
    public class Song
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(100, ErrorMessage = "TitleLength")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "ArtistRequired")]
        [StringLength(100, ErrorMessage = "ArtistLength")]
        public string? Artist { get; set; }

        [Required(ErrorMessage = "FileNameRequired")]
        public string? FileName { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        // Helper methods for sorting
        public static string GetNextSortOrder(string currentSortOrder, string sortBy)
        {
            if (currentSortOrder == $"{sortBy}_asc")
                return $"{sortBy}_desc";
            else
                return $"{sortBy}_asc";
        }
    }
}
