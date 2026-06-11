using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Common.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UsernameRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "UsernameLength")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "PasswordLength")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [Compare("Password", ErrorMessage = "PasswordsDoNotMatch")]
        public string? ConfirmPassword { get; set; }
    }
}
