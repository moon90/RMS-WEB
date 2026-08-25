using System.Collections.Generic;

namespace RMS.Domain.Models.BaseModels
{
    public class KeysetPagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public int? LastSeenId { get; set; } // Can be null if there are no items

        public KeysetPagedResult(IEnumerable<T> items, int pageSize, bool hasNextPage, int? lastSeenId)
        {
            Items = items;
            PageSize = pageSize;
            HasNextPage = hasNextPage;
            LastSeenId = lastSeenId;
        }
    }
}
