using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.Interfaces;
using MusicPortal.Common.Entities;

namespace MusicPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                
                // Не передаємо PasswordHash клієнту!
                var userDtos = users.Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.IsActive,
                    u.IsAdmin,
                    u.CreatedAt,
                    SongsCount = u.Songs?.Count ?? 0
                });

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                
                if (user == null)
                    return NotFound(new { error = "User not found" });

                var userDto = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.IsActive,
                    user.IsAdmin,
                    user.CreatedAt,
                    SongsCount = user.Songs?.Count ?? 0
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/users/5/toggle-status
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var result = await _userService.ToggleUserStatusAsync(id);
                
                if (!result)
                    return NotFound(new { error = "User not found" });

                return Ok(new { message = "User status toggled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Перевірка чи існує
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { error = "User not found" });

                // Не можна видалити адміна
                if (user.IsAdmin)
                    return BadRequest(new { error = "Cannot delete admin user" });

                var result = await _userService.DeleteUserAsync(id);
                
                if (!result)
                    return BadRequest(new { error = "Failed to delete user" });

                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/users/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                
                var stats = new
                {
                    TotalUsers = users.Count(),
                    ActiveUsers = users.Count(u => u.IsActive),
                    InactiveUsers = users.Count(u => !u.IsActive),
                    AdminUsers = users.Count(u => u.IsAdmin),
                    TotalSongs = users.Sum(u => u.Songs?.Count ?? 0)
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
