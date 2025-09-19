using RMS.Domain.Enum;
using System;

namespace RMS.Application.DTOs.AlertDTOs
{
    public class AlertDto
    {
        public int AlertId { get; set; }
        public string Message { get; set; }
        public AlertType Type { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime AlertDate { get; set; }
    }
}
