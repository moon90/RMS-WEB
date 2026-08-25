using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly ITableReservationService _reservationService;

        public ReservationsController(ITableReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("hold")]
        public async Task<IActionResult> CreateHold([FromBody] CreateReservationHoldDto dto, CancellationToken cancellationToken)
        {
            var createdBy = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name ?? "system";
            var response = await _reservationService.CreateHoldAsync(dto, createdBy, cancellationToken);

            if (!response.IsSuccess)
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpPost("{id:guid}/cancel-hold")]
        public async Task<IActionResult> CancelHold(Guid id, CancellationToken cancellationToken)
        {
            var cancelledBy = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name ?? "system";
            var response = await _reservationService.CancelHoldAsync(id, cancelledBy, cancellationToken);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
