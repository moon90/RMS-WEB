using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Infrastructure.IRepositories
{
    public interface ISystemSettingRepository
    {
        Task<IEnumerable<SystemSetting>> GetAllAsync();
        Task<SystemSetting?> GetByKeyAsync(string key);
        Task UpdateAsync(SystemSetting setting);
        Task UpdateRangeAsync(IEnumerable<SystemSetting> settings);
        Task AddAsync(SystemSetting setting);
        Task DeleteAsync(int id);
    }
}
