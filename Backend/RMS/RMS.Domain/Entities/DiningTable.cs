using System;
using RMS.Domain.Enum;
using RMS.Domain.Interfaces;

namespace RMS.Domain.Entities
{
    public class DiningTable : BaseEntity, IMultiTenant
    {
        public int TableID { get; set; }
        public string TableName { get; set; }
        public DiningTableStatusEnum DiningTableStatus { get; set; }
        public int? BranchID { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}