using MusicPortal.Common.Entities;

namespace MusicPortal.BLL.Interfaces
{
    public interface IGenreService
    {
        Task<Genre?> GetGenreByIdAsync(int id);
        Task<Genre?> GetGenreByNameAsync(string name);
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<bool> AddGenreAsync(Genre genre);
        Task<bool> UpdateGenreAsync(Genre genre);
        Task<bool> DeleteGenreAsync(int id);
        Task<bool> GenreExistsAsync(string name);
    }
}
