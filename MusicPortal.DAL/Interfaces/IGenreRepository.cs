using MusicPortal.Common.Entities;

namespace MusicPortal.DAL.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<Genre?> GetByNameAsync(string name);
        Task<Genre?> GetGenreWithSongsAsync(int id);
        Task<IEnumerable<Genre>> GetGenresWithSongCountAsync();
    }
}
