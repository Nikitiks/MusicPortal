namespace MusicPortal.Common.Models
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; }
        public int PageNumber { get; set; } // Для сумісності з PaginationTagHelper
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalCount { get; set; } // Для сумісності з Views

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        
        // Для зворотної сумісності
        public bool HasPrevious => HasPreviousPage;
        public bool HasNext => HasNextPage;
    }
}
