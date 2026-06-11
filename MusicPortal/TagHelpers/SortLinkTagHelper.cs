using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicPortal.Common.Entities;

namespace MusicPortal.TagHelpers
{
    [HtmlTargetElement("sort-link", Attributes = "sort-by, current-sort, link-text")]
    public class SortLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public SortLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public string SortBy { get; set; } = string.Empty;
        public string CurrentSort { get; set; } = string.Empty;
        public string LinkText { get; set; } = string.Empty;
        public string? TitleFilter { get; set; }
        public string? ArtistFilter { get; set; }
        public int? GenreFilter { get; set; }
        public int PageNumber { get; set; } = 1;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            var nextSortOrder = Song.GetNextSortOrder(CurrentSort, SortBy);
            var routeValues = new
            {
                sortOrder = nextSortOrder,
                titleFilter = TitleFilter,
                artistFilter = ArtistFilter,
                genreFilter = GenreFilter,
                pageNumber = PageNumber
            };

            var url = urlHelper.Action(ViewContext.RouteData.Values["action"]?.ToString(), routeValues);

            output.TagName = "a";
            output.Attributes.SetAttribute("href", url);
            output.Attributes.SetAttribute("class", "sort-link");
            output.Content.SetContent(LinkText);

            if (CurrentSort == $"{SortBy}_asc")
            {
                output.Content.AppendHtml(" ▲");
            }
            else if (CurrentSort == $"{SortBy}_desc")
            {
                output.Content.AppendHtml(" ▼");
            }
        }
    }
}