using Microsoft.EntityFrameworkCore;
using MusicPortal.Common.Entities;
using MusicPortal.Common.Models;
using MusicPortal.DAL.Data;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    public class SongRepository : Repository<Song>, ISongRepository
    {
        public SongRepository(MusicPortalContext context) : base(context)
        {
        }

        public async Task<Song?> GetSongWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Genre)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Song>> GetSongsByGenreAsync(int genreId)
        {
            return await _dbSet
                .Include(s => s.Genre)
                .Include(s => s.User)
                .Where(s => s.GenreId == genreId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByUserAsync(int userId)
        {
            return await _dbSet
                .Include(s => s.Genre)
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Genre)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Song> Songs, int TotalCount)> GetFilteredSongsAsync(SortFilterModel filter)
        {
            var query = _dbSet
                .Include(s => s.Genre)
                .Include(s => s.User)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.TitleFilter))
            {
                query = query.Where(s => s.Title != null && s.Title.Contains(filter.TitleFilter));
            }

            if (!string.IsNullOrEmpty(filter.ArtistFilter))
            {
                query = query.Where(s => s.Artist != null && s.Artist.Contains(filter.ArtistFilter));
            }

            if (filter.GenreFilter.HasValue && filter.GenreFilter.Value > 0)
            {
                query = query.Where(s => s.GenreId == filter.GenreFilter.Value);
            }

            var totalCount = await query.CountAsync();

            // Apply sorting
            query = ApplySorting(query, filter.SortOrder);

            // Apply pagination
            var songs = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (songs, totalCount);
        }

        private IQueryable<Song> ApplySorting(IQueryable<Song> query, string sortOrder)
        {
            return sortOrder?.ToLower() switch
            {
                "title_desc" => query.OrderByDescending(s => s.Title),
                "artist_asc" => query.OrderBy(s => s.Artist),
                "artist_desc" => query.OrderByDescending(s => s.Artist),
                "genre_asc" => query.OrderBy(s => s.Genre != null ? s.Genre.Name : ""),
                "genre_desc" => query.OrderByDescending(s => s.Genre != null ? s.Genre.Name : ""),
                "date_asc" => query.OrderBy(s => s.UploadDate),
                "date_desc" => query.OrderByDescending(s => s.UploadDate),
                _ => query.OrderBy(s => s.Title)
            };
        }
    }
}
