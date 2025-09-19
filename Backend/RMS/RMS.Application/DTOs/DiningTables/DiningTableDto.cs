using System;
using RMS.Domain.Enum;

namespace RMS.Application.DTOs.DiningTables
{
    public class DiningTableDto
    {
        public int TableID { get; set; }
        public string TableName { get; set; }
        public bool Status { get; set; }
        public DiningTableStatusEnum DiningTableStatus { get; set; }
    }
}