using MusicPortal.BLL.DTOs;
using MusicPortal.BLL.Interfaces;
using MusicPortal.BLL.Mappers;
using MusicPortal.Common.Models;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.BLL.Services
{
    /// <summary>
    /// Реалізація сервісу для роботи з піснями через DTO
    /// Демонструє використання трансферних моделей для ізоляції шарів
    /// </summary>
    public class SongDtoService : ISongDtoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SongDtoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SongDto?> GetSongByIdAsync(int id)
        {
            var song = await _unitOfWork.Songs.GetSongWithDetailsAsync(id);
            return song?.ToDto();
        }

        public async Task<IEnumerable<SongDto>> GetAllSongsAsync()
        {
            var songs = await _unitOfWork.Songs.GetSongsWithDetailsAsync();
            return songs.ToDto();
        }

        public async Task<IEnumerable<SongDto>> GetSongsByGenreAsync(int genreId)
        {
            var songs = await _unitOfWork.Songs.GetSongsByGenreAsync(genreId);
            return songs.ToDto();
        }

        public async Task<IEnumerable<SongDto>> GetSongsByUserAsync(int userId)
        {
            var songs = await _unitOfWork.Songs.GetSongsByUserAsync(userId);
            return songs.ToDto();
        }

        public async Task<(IEnumerable<SongDto> Songs, PaginationModel Pagination)> GetFilteredSongsAsync(SortFilterModel filter)
        {
            var (songs, totalCount) = await _unitOfWork.Songs.GetFilteredSongsAsync(filter);

            var pagination = new PaginationModel
            {
                CurrentPage = filter.PageNumber,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItems = totalCount,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };

            return (songs.ToDto(), pagination);
        }

        public async Task<SongDto> CreateSongAsync(CreateSongDto createDto)
        {
            var song = createDto.ToEntity();
            
            await _unitOfWork.Songs.AddAsync(song);
            await _unitOfWork.SaveChangesAsync();

            // Завантажуємо створену пісню з деталями
            var createdSong = await _unitOfWork.Songs.GetSongWithDetailsAsync(song.Id);
            return createdSong!.ToDto();
        }

        public async Task<SongDto?> UpdateSongAsync(UpdateSongDto updateDto)
        {
            var song = await _unitOfWork.Songs.GetByIdAsync(updateDto.Id);
            if (song == null)
                return null;

            updateDto.UpdateEntity(song);
            _unitOfWork.Songs.Update(song);
            await _unitOfWork.SaveChangesAsync();

            var updatedSong = await _unitOfWork.Songs.GetSongWithDetailsAsync(song.Id);
            return updatedSong?.ToDto();
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

        public async Task<bool> SongExistsAsync(int id)
        {
            return await _unitOfWork.Songs.AnyAsync(s => s.Id == id);
        }
    }
}
