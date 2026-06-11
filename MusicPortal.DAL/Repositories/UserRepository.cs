using Microsoft.EntityFrameworkCore;
using MusicPortal.Common.Entities;
using MusicPortal.DAL.Data;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MusicPortalContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithSongsAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Songs)
                .ThenInclude(s => s.Genre)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAdminUsersAsync()
        {
            return await _dbSet.Where(u => u.IsAdmin).ToListAsync();
        }
    }
}
