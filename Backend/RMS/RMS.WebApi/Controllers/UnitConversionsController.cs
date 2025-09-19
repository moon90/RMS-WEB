
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UnitConversionsController : ControllerBase
    {
        private readonly IUnitConversionService _unitConversionService;

        public UnitConversionsController(IUnitConversionService unitConversionService)
        {
            _unitConversionService = unitConversionService;
        }

        [HttpGet]
        [Authorize(Policy = "UNIT_CONVERSION_VIEW")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitConversionService.GetAllUnitConversionsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UNIT_CONVERSION_VIEW")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _unitConversionService.GetUnitConversionByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Policy = "UNIT_CONVERSION_CREATE")]
        public async Task<IActionResult> Create([FromBody] CreateUnitConversionDto createDto)
        {
            var result = await _unitConversionService.CreateUnitConversionAsync(createDto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data.UnitConversionID }, result) : BadRequest(result);
        }

        [HttpGet("convert")]
        [Authorize(Policy = "UNIT_CONVERSION_VIEW")]
        public async Task<IActionResult> ConvertUnits([FromQuery] int fromUnitId, [FromQuery] int toUnitId, [FromQuery] decimal value)
        {
            var result = await _unitConversionService.ConvertUnitsAsync(fromUnitId, toUnitId, value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
