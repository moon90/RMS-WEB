
using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ISaleService
    {
        Task<List<SaleDto>> GetSalesAsync();
        Task<SaleDto> GetSaleAsync(int id);
        Task<ResponseDto<SaleDto>> CreateSaleAsync(CreateSaleDto saleDto);
    }
}
