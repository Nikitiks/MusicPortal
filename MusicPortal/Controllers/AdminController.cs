using Microsoft.AspNetCore.Mvc;
using MusicPortal.Common.Entities;
using MusicPortal.BLL.Interfaces;
using Microsoft.Extensions.Localization;

namespace MusicPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;
        private readonly IStringLocalizer<AdminController> _localizer;

        public AdminController(
            IUserService userService,
            ISongService songService,
            IGenreService genreService,
            IStringLocalizer<AdminController> localizer)
        {
            _userService = userService;
            _songService = songService;
            _genreService = genreService;
            _localizer = localizer;
        }

        private bool IsAdmin()
        {
            var isAdminStr = HttpContext.Session.GetString("IsAdmin");
            return isAdminStr == "True";
        }

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            return View();
        }

        public async Task<IActionResult> Users()
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            await _userService.ToggleUserStatusAsync(id);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            await _userService.ToggleUserStatusAsync(id);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Genres()
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            var genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = genres;
            return View(genres);
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(string name)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");

            if (!string.IsNullOrEmpty(name))
            {
                var genre = new Genre { Name = name };
                await _genreService.AddGenreAsync(genre);
            }
            return RedirectToAction("Genres");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            await _genreService.DeleteGenreAsync(id);
            return RedirectToAction("Genres");
        }

        public async Task<IActionResult> Songs()
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            var songs = await _songService.GetAllSongsAsync();
            var genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = genres;
            return View(songs);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSong(int id)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");
            await _songService.DeleteSongAsync(id);
            return RedirectToAction("Songs");
        }

        [HttpPost]
        public async Task<IActionResult> AddSongAdmin(IFormFile file, string title, string artist, int genreId, int userId)
        {
            if (!IsAdmin()) return RedirectToAction("AccessDenied", "Account");

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
                    UserId = userId
                };

                await _songService.AddSongAsync(song);
                TempData["SuccessMessage"] = _localizer["SongAddedSuccessfully"];
            }
            else
            {
                TempData["ErrorMessage"] = _localizer["PleaseFillAllFields"];
            }

            return RedirectToAction("Songs");
        }
    }
}
