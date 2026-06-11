using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Common.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UsernameRequired")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        public string? Password { get; set; }
    }
}
