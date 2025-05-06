using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Queries
{
    public class PagedQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? OrderBy { get; set; }
        public bool IsDescending { get; set; } = false;

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append($"{nameof(PageNumber)}:{PageNumber}");
            stringBuilder.Append($"-{nameof(PageSize)}:{PageSize}");

            if (!string.IsNullOrEmpty(OrderBy))
            {
                string typeOrder = IsDescending ? "order descending by " : "order by ";
                stringBuilder.Append($"-{typeOrder}:{OrderBy}");
            }

            return stringBuilder.ToString();
        }
    }
}
