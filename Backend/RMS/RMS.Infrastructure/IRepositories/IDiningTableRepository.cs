using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces; // Correct namespace for IBaseRepository

namespace RMS.Infrastructure.IRepositories
{
    public interface IDiningTableRepository : IBaseRepository<DiningTable>
    {
        // Add any DiningTable-specific methods here that are not covered by IBaseRepository
        // For example:
        Task<IEnumerable<DiningTable>> GetAvailableTablesAsync();
    }
}