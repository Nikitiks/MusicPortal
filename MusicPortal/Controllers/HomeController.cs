using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MusicPortal.Common.Models;
using MusicPortal.BLL.Interfaces;

namespace MusicPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(
            ISongService songService, 
            IGenreService genreService,
            IStringLocalizer<HomeController> localizer)
        {
            _songService = songService;
            _genreService = genreService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var songs = await _songService.GetAllSongsAsync();
            var genres = await _genreService.GetAllGenresAsync();

            ViewBag.Genres = genres;
            return View(songs.Take(6).ToList());
        }

        public async Task<IActionResult> Songs(string sortOrder = "title_asc",
            string titleFilter = "", string artistFilter = "", int? genreFilter = null, int pageNumber = 1)
        {
            var filter = new SortFilterModel
            {
                SortOrder = sortOrder,
                TitleFilter = titleFilter,
                ArtistFilter = artistFilter,
                GenreFilter = genreFilter,
                PageNumber = pageNumber,
                PageSize = 6
            };

            var (songs, pagination) = await _songService.GetFilteredSongsAsync(filter);
            var genres = await _genreService.GetAllGenresAsync();

            ViewBag.Genres = genres;
            ViewBag.SelectedGenre = genreFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleFilter = titleFilter;
            ViewBag.ArtistFilter = artistFilter;
            ViewBag.PaginationModel = pagination;
            ViewBag.SortFilterModel = filter;

            return View(songs);
        }

        public async Task<IActionResult> Download(int id)
        {
            var song = await _songService.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var path = Path.Combine(uploadsPath, song.FileName ?? "song.mp3");
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "audio/mpeg", $"{song.Artist} - {song.Title}.mp3");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
