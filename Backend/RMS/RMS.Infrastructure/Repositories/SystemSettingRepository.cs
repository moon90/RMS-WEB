using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly RestaurantDbContext _context;

        public SystemSettingRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SystemSetting>> GetAllAsync()
        {
            return await _context.SystemSettings.ToListAsync();
        }

        public async Task<SystemSetting?> GetByKeyAsync(string key)
        {
            return await _context.SystemSettings.FirstOrDefaultAsync(s => s.SettingKey == key);
        }

        public async Task UpdateAsync(SystemSetting setting)
        {
            _context.SystemSettings.Update(setting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<SystemSetting> settings)
        {
            _context.SystemSettings.UpdateRange(settings);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(SystemSetting setting)
        {
            await _context.SystemSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var setting = await _context.SystemSettings.FindAsync(id);
            if (setting != null)
            {
                _context.SystemSettings.Remove(setting);
                await _context.SaveChangesAsync();
            }
        }
    }
}
