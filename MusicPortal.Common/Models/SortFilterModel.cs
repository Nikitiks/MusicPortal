namespace MusicPortal.Common.Models
{
    public class SortFilterModel
    {
        public string? TitleFilter { get; set; }
        public string? ArtistFilter { get; set; }
        public int? GenreFilter { get; set; }
        public string SortOrder { get; set; } = "title_asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
