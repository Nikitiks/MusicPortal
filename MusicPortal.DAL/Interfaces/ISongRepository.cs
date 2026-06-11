using MusicPortal.Common.Entities;
using MusicPortal.Common.Models;

namespace MusicPortal.DAL.Interfaces
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<Song?> GetSongWithDetailsAsync(int id);
        Task<IEnumerable<Song>> GetSongsByGenreAsync(int genreId);
        Task<IEnumerable<Song>> GetSongsByUserAsync(int userId);
        Task<IEnumerable<Song>> GetSongsWithDetailsAsync();
        Task<(IEnumerable<Song> Songs, int TotalCount)> GetFilteredSongsAsync(SortFilterModel filter);
    }
}
