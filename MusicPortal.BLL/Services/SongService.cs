using MusicPortal.BLL.Interfaces;
using MusicPortal.Common.Entities;
using MusicPortal.Common.Models;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.BLL.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SongService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            return await _unitOfWork.Songs.GetSongWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _unitOfWork.Songs.GetSongsWithDetailsAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByGenreAsync(int genreId)
        {
            return await _unitOfWork.Songs.GetSongsByGenreAsync(genreId);
        }

        public async Task<IEnumerable<Song>> GetSongsByUserAsync(int userId)
        {
            return await _unitOfWork.Songs.GetSongsByUserAsync(userId);
        }

        public async Task<(IEnumerable<Song> Songs, PaginationModel Pagination)> GetFilteredSongsAsync(SortFilterModel filter)
        {
            var (songs, totalCount) = await _unitOfWork.Songs.GetFilteredSongsAsync(filter);

            var pagination = new PaginationModel
            {
                CurrentPage = filter.PageNumber,
                PageNumber = filter.PageNumber, // Для сумісності
                PageSize = filter.PageSize,
                TotalItems = totalCount,
                TotalCount = totalCount, // Для сумісності з Views
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };

            return (songs, pagination);
        }

        public async Task<bool> AddSongAsync(Song song)
        {
            song.UploadDate = DateTime.Now;
            await _unitOfWork.Songs.AddAsync(song);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSongAsync(Song song)
        {
            _unitOfWork.Songs.Update(song);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSongAsync(int id)
        {
            var song = await _unitOfWork.Songs.GetByIdAsync(id);
            if (song == null)
                return false;

            _unitOfWork.Songs.Remove(song);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
