using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IBranchService
    {
        Task<ResponseDto<List<BranchDto>>> GetAllBranchesAsync();
        Task<ResponseDto<BranchDto>> GetBranchByIdAsync(int id);
        Task<ResponseDto<BranchDto>> CreateBranchAsync(CreateBranchDto branchDto);
        Task<ResponseDto<bool>> DeleteBranchAsync(int id);
    }
}
