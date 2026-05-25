using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.DTOs.SystemSettings;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingService _settingService;

        public SystemSettingsController(ISystemSettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _settingService.GetAllSettingsAsync();
            return Ok(result);
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var result = await _settingService.GetSettingByKeyAsync(key);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSystemSettingDto updateDto)
        {
            var result = await _settingService.UpdateSettingAsync(updateDto);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBulk([FromBody] IEnumerable<UpdateSystemSettingDto> updateDtos)
        {
            var result = await _settingService.UpdateSettingsAsync(updateDtos);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemSettingDto createDto)
        {
            var result = await _settingService.CreateSettingAsync(createDto);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _settingService.DeleteSettingAsync(id);
            return Ok(result);
        }
    }
}
