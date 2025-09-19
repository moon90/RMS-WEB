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
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? Status { get; set; }

        // Internal properties for mapping to existing logic
        public string? OrderBy
        {
            get => SortColumn;
            set => SortColumn = value;
        }

        public bool IsDescending
        {
            get => SortDirection?.ToLower() == "desc";
            set => SortDirection = value ? "desc" : "asc";
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append($"_page:{PageNumber}");
            stringBuilder.Append($"-limit:{PageSize}");

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                stringBuilder.Append($"-q:{SearchQuery}");
            }

            if (!string.IsNullOrEmpty(SortColumn))
            {
                stringBuilder.Append($"-sort:{SortColumn}");
                stringBuilder.Append($"-order:{(SortDirection?.ToLower() == "desc" ? "desc" : "asc")}");
            }

            if (!string.IsNullOrEmpty(Status))
            {
                stringBuilder.Append($"-status:{Status}");
            }

            return stringBuilder.ToString();
        }
    }
}