using RMS.Domain.Enum;
using System;

namespace RMS.Application.DTOs.DiningTables
{
    public class UpdateDiningTableDto
    {
        public int TableID { get; set; } // Required for identifying the table to update
        public string TableName { get; set; }
        public bool Status { get; set; }
        public DiningTableStatusEnum DiningTableStatus { get; set; }
    }
}