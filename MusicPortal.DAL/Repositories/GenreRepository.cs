using Microsoft.EntityFrameworkCore;
using MusicPortal.Common.Entities;
using MusicPortal.DAL.Data;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MusicPortalContext context) : base(context)
        {
        }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task<Genre?> GetGenreWithSongsAsync(int id)
        {
            return await _dbSet
                .Include(g => g.Songs)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Genre>> GetGenresWithSongCountAsync()
        {
            return await _dbSet
                .Include(g => g.Songs)
                .ToListAsync();
        }
    }
}
