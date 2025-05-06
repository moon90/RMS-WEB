using RMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Persistences
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isDisposed = false;

        private readonly RestaurantDbContext _context;

        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Disposing(!_isDisposed);
        }

        private void Disposing(bool isDisposing)
        {
            if (isDisposing)
            {
                _context.Dispose();
                _isDisposed = isDisposing;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
