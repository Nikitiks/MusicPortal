using System.ComponentModel.DataAnnotations;

namespace MusicPortal.ViewModels
{
    /// <summary>
    /// ViewModel для фільтрації та пошуку пісень
    /// </summary>
    public class SongFilterViewModel
    {
        [Display(Name = "Пошук за назвою")]
        [StringLength(100, ErrorMessage = "Максимум 100 символів")]
        public string? TitleFilter { get; set; }

        [Display(Name = "Пошук за виконавцем")]
        [StringLength(100, ErrorMessage = "Максимум 100 символів")]
        public string? ArtistFilter { get; set; }

        [Display(Name = "Жанр")]
        public int? GenreFilter { get; set; }

        [Display(Name = "Сортування")]
        public string SortOrder { get; set; } = "title_asc";

        [Range(1, int.MaxValue, ErrorMessage = "Номер сторінки має бути позитивним")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Розмір сторінки має бути від 1 до 100")]
        public int PageSize { get; set; } = 6;
    }
}
