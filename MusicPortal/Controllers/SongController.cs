using Microsoft.AspNetCore.Mvc;
using MusicPortal.Common.Entities;
using MusicPortal.BLL.Interfaces;
using Microsoft.Extensions.Localization;

namespace MusicPortal.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;
        private readonly IStringLocalizer<SongController> _localizer;

        public SongController(
            ISongService songService, 
            IGenreService genreService,
            IStringLocalizer<SongController> localizer)
        {
            _songService = songService;
            _genreService = genreService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = genres;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IFormFile file, string title, string artist, int genreId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (file != null && file.Length > 0 && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(artist))
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var song = new Song
                {
                    Title = title,
                    Artist = artist,
                    FileName = fileName,
                    GenreId = genreId,
                    UserId = userId.Value
                };

                await _songService.AddSongAsync(song);
                ViewBag.SuccessMessage = _localizer["SongAddedSuccessfully"];
                ModelState.Clear();
            }
            else
            {
                ViewBag.ErrorMessage = _localizer["PleaseFillAllFields"];
            }

            var genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = genres;
            return View();
        }
    }
}
