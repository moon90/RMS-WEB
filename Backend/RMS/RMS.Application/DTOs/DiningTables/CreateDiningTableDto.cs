using RMS.Domain.Enum;
using System;

namespace RMS.Application.DTOs.DiningTables
{
    public class CreateDiningTableDto
    {
        public string TableName { get; set; }
        public bool Status { get; set; } = true; // Default to active
        public DiningTableStatusEnum DiningTableStatus { get; set; }
    }
}