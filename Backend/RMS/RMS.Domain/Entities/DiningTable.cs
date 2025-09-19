using System;
using RMS.Domain.Enum;

namespace RMS.Domain.Entities
{
    public class DiningTable : BaseEntity
    {
        public int TableID { get; set; }
        public string TableName { get; set; }
        // Status is already in BaseEntity, but if you need a specific table status (e.g., "Occupied", "Available", "Reserved"), you might add it here.
        // For now, I'll rely on BaseEntity.Status.
        public DiningTableStatusEnum DiningTableStatus { get; set; }
    }
}