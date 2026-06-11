using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Common.Entities;

namespace MusicPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            try
            {
                var genres = await _genreService.GetAllGenresAsync();
                
                var genreDtos = genres.Select(g => new
                {
                    g.Id,
                    g.Name,
                    SongsCount = g.Songs?.Count ?? 0
                });

                return Ok(genreDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            try
            {
                var genre = await _genreService.GetGenreByIdAsync(id);
                
                if (genre == null)
                    return NotFound(new { error = "Genre not found" });

                var genreDto = new
                {
                    genre.Id,
                    genre.Name,
                    SongsCount = genre.Songs?.Count ?? 0
                };

                return Ok(genreDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/genres
        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] GenreCreateModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return BadRequest(new { error = "Genre name is required" });

                // Перевірка на існування
                var existing = await _genreService.GetAllGenresAsync();
                if (existing.Any(g => g.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                    return BadRequest(new { error = "Genre already exists" });

                var genre = new Genre
                {
                    Name = model.Name.Trim()
                };

                var created = await _genreService.AddGenreAsync(genre);

                var genreDto = new
                {
                    created.Id,
                    created.Name,
                    SongsCount = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, genreDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GenreUpdateModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return BadRequest(new { error = "Genre name is required" });

                var genre = await _genreService.GetGenreByIdAsync(id);
                if (genre == null)
                    return NotFound(new { error = "Genre not found" });

                // Перевірка на дублікат
                var existing = await _genreService.GetAllGenresAsync();
                if (existing.Any(g => g.Id != id && g.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                    return BadRequest(new { error = "Genre with this name already exists" });

                genre.Name = model.Name.Trim();
                var result = await _genreService.UpdateGenreAsync(genre);

                if (!result)
                    return BadRequest(new { error = "Failed to update genre" });

                return Ok(new { message = "Genre updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var genre = await _genreService.GetGenreByIdAsync(id);
                if (genre == null)
                    return NotFound(new { error = "Genre not found" });

                // Не можна видалити якщо є пісні
                if (genre.Songs != null && genre.Songs.Any())
                    return BadRequest(new { error = "Cannot delete genre with songs" });

                var result = await _genreService.DeleteGenreAsync(id);
                
                if (!result)
                    return BadRequest(new { error = "Failed to delete genre" });

                return Ok(new { message = "Genre deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    // Models for API requests
    public class GenreCreateModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GenreUpdateModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
