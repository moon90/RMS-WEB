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

        public string? SearchQuery { get; set; }
        public string? OrderBy { get; set; }
        public bool IsDescending { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append($"{nameof(PageNumber)}:{PageNumber}");
            stringBuilder.Append($"-{nameof(PageSize)}:{PageSize}");

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                stringBuilder.Append($"-{nameof(SearchQuery)}:{SearchQuery}");
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                stringBuilder.Append($"-{nameof(OrderBy)}:{OrderBy}");
                stringBuilder.Append($"-{nameof(IsDescending)}:{IsDescending}");
            }

            return stringBuilder.ToString();
        }
    }
}
