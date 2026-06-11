using MusicPortal.Common.Entities;
using MusicPortal.Common.Models;

namespace MusicPortal.BLL.Interfaces
{
    public interface ISongService
    {
        Task<Song?> GetSongByIdAsync(int id);
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<IEnumerable<Song>> GetSongsByGenreAsync(int genreId);
        Task<IEnumerable<Song>> GetSongsByUserAsync(int userId);
        Task<(IEnumerable<Song> Songs, PaginationModel Pagination)> GetFilteredSongsAsync(SortFilterModel filter);
        Task<bool> AddSongAsync(Song song);
        Task<bool> UpdateSongAsync(Song song);
        Task<bool> DeleteSongAsync(int id);
    }
}
