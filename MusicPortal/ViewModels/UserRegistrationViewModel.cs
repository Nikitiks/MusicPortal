using System.ComponentModel.DataAnnotations;

namespace MusicPortal.ViewModels
{
    /// <summary>
    /// ViewModel для реєстрації користувача з розширеною валідацією
    /// </summary>
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ім'я користувача має бути від 3 до 50 символів")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Тільки латинські літери, цифри та підкреслення")]
        [Display(Name = "Ім'я користувача")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний формат email")]
        [StringLength(100, ErrorMessage = "Максимум 100 символів")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути від 6 до 100 символів")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
            ErrorMessage = "Пароль має містити мінімум одну велику літеру, одну малу літеру та одну цифру")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Я погоджуюсь з умовами використання")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Потрібно погодитись з умовами")]
        public bool AcceptTerms { get; set; }
    }
}
