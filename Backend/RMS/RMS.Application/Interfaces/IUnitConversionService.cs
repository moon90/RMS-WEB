
using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IUnitConversionService
    {
        Task<ResponseDto<UnitConversionDto>> CreateUnitConversionAsync(CreateUnitConversionDto unitConversionDto);
        Task<ResponseDto<List<UnitConversionDto>>> GetAllUnitConversionsAsync();
        Task<ResponseDto<UnitConversionDto>> GetUnitConversionByIdAsync(int id);
        Task<ResponseDto<decimal>> ConvertUnitsAsync(int fromUnitId, int toUnitId, decimal value);
    }
}
