using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMS.Domain.Interfaces;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Repositories;
using System.Collections.Concurrent;
using System.Data;

namespace RMS.Infrastructure.Persistences
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isDisposed = false;
        private IDbContextTransaction? _currentTransaction;
        private int _transactionDepth = 0;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        private readonly RestaurantDbContext _context;
        private readonly ITenantService _tenantService;

        public UnitOfWork(RestaurantDbContext context, ITenantService tenantService)
        {
            _context = context;
            _tenantService = tenantService;
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (IBaseRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity), _ => 
                new BaseRepository<TEntity>(_context, _tenantService));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _isDisposed = true;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = await _context.Database.BeginTransactionAsync();
            }
            
            _transactionDepth++;
            return _currentTransaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel);
            }
            
            _transactionDepth++;
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                _transactionDepth--;
                
                if (_transactionDepth == 0)
                {
                    await _context.SaveChangesAsync();
                    await _currentTransaction.CommitAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                try
                {
                    await _currentTransaction.RollbackAsync();
                }
                finally
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                    _transactionDepth = 0;
                }
            }
        }
    }
}
