using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Common.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        [StringLength(50, ErrorMessage = "NameLength")]
        public string? Name { get; set; }

        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
