using System.ComponentModel.DataAnnotations;

namespace MusicPortal.ViewModels
{
    /// <summary>
    /// ViewModel для входу користувача
    /// </summary>
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я користувача")]
        [Display(Name = "Ім'я користувача")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запам'ятати мене")]
        public bool RememberMe { get; set; }
    }
}
