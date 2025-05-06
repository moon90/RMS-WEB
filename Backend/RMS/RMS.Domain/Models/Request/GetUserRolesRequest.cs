using RMS.Core.Enum.SortField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.Request
{
    public class GetUserRolesRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? EmailFilter { get; set; }

        public UserRoleSortField? SortBy { get; set; }

        public string? SortDirection { get; set; }
    }
}
