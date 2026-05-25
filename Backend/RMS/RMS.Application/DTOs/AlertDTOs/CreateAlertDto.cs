using RMS.Domain.Enum;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs.AlertDTOs
{
    public class CreateAlertDto
    {
        public string Message { get; set; }
        public AlertType Type { get; set; }
    }
}
