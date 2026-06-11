using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using MusicPortal.Validation;

namespace MusicPortal.ViewModels
{
    /// <summary>
    /// ViewModel для завантаження пісні з валідацією
    /// </summary>
    public class SongUploadViewModel
    {
        [Required(ErrorMessage = "Назва пісні обов'язкова")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Назва має бути від 1 до 100 символів")]
        [Display(Name = "Назва пісні")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Виконавець обов'язковий")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Ім'я виконавця має бути від 1 до 100 символів")]
        [Display(Name = "Виконавець")]
        public string Artist { get; set; } = string.Empty;

        [Required(ErrorMessage = "Виберіть жанр")]
        [Range(1, int.MaxValue, ErrorMessage = "Виберіть жанр зі списку")]
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Виберіть файл пісні")]
        [DataType(DataType.Upload)]
        [Display(Name = "Аудіо файл")]
        [AllowedFileExtensions(".mp3", ".wav", ".flac", ".m4a")]
        [MaxFileSize(10 * 1024 * 1024)] // 10 MB
        public IFormFile? File { get; set; }
    }
}
