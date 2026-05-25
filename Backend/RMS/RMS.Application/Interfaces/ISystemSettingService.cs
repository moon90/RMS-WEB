using RMS.Application.DTOs;
using RMS.Application.DTOs.SystemSettings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ISystemSettingService
    {
        Task<ResponseDto<IEnumerable<SystemSettingDto>>> GetAllSettingsAsync();
        Task<ResponseDto<SystemSettingDto>> GetSettingByKeyAsync(string key);
        Task<ResponseDto<bool>> UpdateSettingAsync(UpdateSystemSettingDto updateDto);
        Task<ResponseDto<bool>> UpdateSettingsAsync(IEnumerable<UpdateSystemSettingDto> updateDtos);
        Task<ResponseDto<SystemSettingDto>> CreateSettingAsync(SystemSettingDto createDto);
        Task<ResponseDto<bool>> DeleteSettingAsync(int id);
    }
}
