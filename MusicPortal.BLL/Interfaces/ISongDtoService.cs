using MusicPortal.BLL.DTOs;
using MusicPortal.Common.Models;

namespace MusicPortal.BLL.Interfaces
{
    /// <summary>
    /// Сервіс для роботи з піснями через DTO
    /// </summary>
    public interface ISongDtoService
    {
        Task<SongDto?> GetSongByIdAsync(int id);
        Task<IEnumerable<SongDto>> GetAllSongsAsync();
        Task<IEnumerable<SongDto>> GetSongsByGenreAsync(int genreId);
        Task<IEnumerable<SongDto>> GetSongsByUserAsync(int userId);
        Task<(IEnumerable<SongDto> Songs, PaginationModel Pagination)> GetFilteredSongsAsync(SortFilterModel filter);
        Task<SongDto> CreateSongAsync(CreateSongDto createDto);
        Task<SongDto?> UpdateSongAsync(UpdateSongDto updateDto);
        Task<bool> DeleteSongAsync(int id);
        Task<bool> SongExistsAsync(int id);
    }
}
