using System;
using RMS.Domain.Enum;

namespace RMS.Application.DTOs.DiningTables
{
    public class UpdateDiningTableStatusDto
    {
        public int TableID { get; set; }
        public bool Status { get; set; }
        public DiningTableStatusEnum DiningTableStatus { get; set; }
    }
}