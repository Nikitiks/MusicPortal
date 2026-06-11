using MusicPortal.BLL.Interfaces;
using MusicPortal.Common.Entities;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _unitOfWork.Genres.GetByIdAsync(id);
        }

        public async Task<Genre?> GetGenreByNameAsync(string name)
        {
            return await _unitOfWork.Genres.GetByNameAsync(name);
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _unitOfWork.Genres.GetAllAsync();
        }

        public async Task<bool> AddGenreAsync(Genre genre)
        {
            if (await GenreExistsAsync(genre.Name!))
                return false;

            await _unitOfWork.Genres.AddAsync(genre);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateGenreAsync(Genre genre)
        {
            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGenreAsync(int id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);
            if (genre == null)
                return false;

            _unitOfWork.Genres.Remove(genre);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GenreExistsAsync(string name)
        {
            var genre = await _unitOfWork.Genres.GetByNameAsync(name);
            return genre != null;
        }
    }
}
