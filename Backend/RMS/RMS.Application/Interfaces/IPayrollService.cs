using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IPayrollService
    {
        Task<ResponseDto<List<PayrollDto>>> GetAllPayrollsAsync();
        Task<ResponseDto<PayrollDto>> RunPayrollAiAsync(CreatePayrollDto runDto);
    }
}
