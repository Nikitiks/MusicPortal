using Microsoft.EntityFrameworkCore.Storage;
using MusicPortal.DAL.Data;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicPortalContext _context;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; }
        public ISongRepository Songs { get; }
        public IGenreRepository Genres { get; }

        public UnitOfWork(MusicPortalContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Songs = new SongRepository(_context);
            Genres = new GenreRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
