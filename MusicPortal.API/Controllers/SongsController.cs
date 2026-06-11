using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Common.Models;

namespace MusicPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;

        public SongsController(ISongService songService, IGenreService genreService)
        {
            _songService = songService;
            _genreService = genreService;
        }

        // GET: api/songs
        [HttpGet]
        public async Task<ActionResult<object>> GetAll([FromQuery] string? title, [FromQuery] string? artist, [FromQuery] int? genreId)
        {
            try
            {
                var filter = new SortFilterModel
                {
                    TitleFilter = title,
                    ArtistFilter = artist,
                    GenreId = genreId,
                    PageSize = 1000 // Всі пісні для адміна
                };

                var (songs, totalCount) = await _songService.GetFilteredSongsAsync(filter);

                var songDtos = songs.Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Artist,
                    s.FileName,
                    s.UploadDate,
                    GenreId = s.GenreId,
                    GenreName = s.Genre?.Name,
                    UserId = s.UserId,
                    Username = s.User?.Username
                });

                return Ok(new
                {
                    songs = songDtos,
                    totalCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            try
            {
                var song = await _songService.GetSongByIdAsync(id);
                
                if (song == null)
                    return NotFound(new { error = "Song not found" });

                var songDto = new
                {
                    song.Id,
                    song.Title,
                    song.Artist,
                    song.FileName,
                    song.UploadDate,
                    GenreId = song.GenreId,
                    GenreName = song.Genre?.Name,
                    UserId = song.UserId,
                    Username = song.User?.Username
                };

                return Ok(songDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var song = await _songService.GetSongByIdAsync(id);
                if (song == null)
                    return NotFound(new { error = "Song not found" });

                var result = await _songService.DeleteSongAsync(id);
                
                if (!result)
                    return BadRequest(new { error = "Failed to delete song" });

                return Ok(new { message = "Song deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/songs/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var songs = await _songService.GetAllSongsAsync();
                var genres = await _genreService.GetAllGenresAsync();

                var stats = new
                {
                    TotalSongs = songs.Count(),
                    TotalGenres = genres.Count(),
                    SongsByGenre = genres.Select(g => new
                    {
                        GenreName = g.Name,
                        Count = songs.Count(s => s.GenreId == g.Id)
                    }).OrderByDescending(x => x.Count),
                    RecentSongs = songs
                        .OrderByDescending(s => s.UploadDate)
                        .Take(5)
                        .Select(s => new
                        {
                            s.Id,
                            s.Title,
                            s.Artist,
                            s.UploadDate
                        })
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
