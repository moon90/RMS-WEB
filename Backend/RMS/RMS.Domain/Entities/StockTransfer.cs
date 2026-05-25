using System;
using System.Collections.Generic;
using RMS.Core.Enum;

namespace RMS.Domain.Entities
{
    public class StockTransfer : BaseEntity
    {
        public int StockTransferID { get; set; }
        public DateTime TransferDate { get; set; }
        public int FromBranchID { get; set; }
        public int ToBranchID { get; set; }
        public TransferStatus Status { get; set; } = TransferStatus.Pending;
        public string? Remarks { get; set; }
        public string? TransferNumber { get; set; }

        public virtual Branch FromBranch { get; set; }
        public virtual Branch ToBranch { get; set; }
        public virtual ICollection<StockTransferDetail> Details { get; set; } = new List<StockTransferDetail>();
    }
}
