using System;
using System.ComponentModel.DataAnnotations;

namespace RMS.Application.DTOs
{
    public class CancelReservationDto
    {
        [MaxLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters.")]
        public string? Reason { get; set; }
    }
}
