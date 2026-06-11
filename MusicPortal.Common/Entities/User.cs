using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Common.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UsernameRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "UsernameLength")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "PasswordLength")]
        public string? Password { get; set; }

        public bool IsActive { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
