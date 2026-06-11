using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicPortal.Common.Models;

namespace MusicPortal.TagHelpers
{
    [HtmlTargetElement("pagination", Attributes = "pagination-model, sort-filter-model")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public PaginationModel PaginationModel { get; set; } = null!;
        public SortFilterModel SortFilterModel { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var currentPage = PaginationModel.PageNumber > 0 ? PaginationModel.PageNumber : PaginationModel.CurrentPage;

            if (PaginationModel.HasPreviousPage)
            {
                var li = CreatePageItem(currentPage - 1, "&laquo;", false);
                ul.InnerHtml.AppendHtml(li);
            }

            for (int i = 1; i <= PaginationModel.TotalPages; i++)
            {
                var li = CreatePageItem(i, i.ToString(), i == currentPage);
                ul.InnerHtml.AppendHtml(li);
            }

            if (PaginationModel.HasNextPage)
            {
                var li = CreatePageItem(currentPage + 1, "&raquo;", false);
                ul.InnerHtml.AppendHtml(li);
            }

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreatePageItem(int pageNumber, string text, bool isActive)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (isActive) li.AddCssClass("active");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var routeValues = new
            {
                sortOrder = SortFilterModel.SortOrder,
                titleFilter = SortFilterModel.TitleFilter,
                artistFilter = SortFilterModel.ArtistFilter,
                genreFilter = SortFilterModel.GenreFilter,
                pageNumber = pageNumber
            };

            a.Attributes["href"] = urlHelper.Action(ViewContext.RouteData.Values["action"]?.ToString(), routeValues);
            a.InnerHtml.AppendHtml(text);

            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}