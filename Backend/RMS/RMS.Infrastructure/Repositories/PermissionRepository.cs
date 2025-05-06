using RMS.Domain.Entities;
using RMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        public Task AddPermissionAsync(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task DeletePermissionAsync(int permissionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Permission> GetPermissionByIdAsync(int permissionId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePermissionAsync(Permission permission)
        {
            throw new NotImplementedException();
        }
    }
}
