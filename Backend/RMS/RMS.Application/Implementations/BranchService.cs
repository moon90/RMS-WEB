using RMS.Infrastructure.IRepositories;
using AutoMapper;
using RMS.Application.DTOs;
using RMS.Domain.Interfaces;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.Application.Interfaces;

namespace RMS.Application.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<BranchDto>>> GetAllBranchesAsync()
        {
            var branches = await _branchRepository.GetAllAsync();
            return ResponseDto<List<BranchDto>>.CreateSuccessResponse(_mapper.Map<List<BranchDto>>(branches));
        }

        public async Task<ResponseDto<BranchDto>> GetBranchByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null) return ResponseDto<BranchDto>.CreateErrorResponse("Branch not found.");
            return ResponseDto<BranchDto>.CreateSuccessResponse(_mapper.Map<BranchDto>(branch));
        }

        public async Task<ResponseDto<BranchDto>> CreateBranchAsync(CreateBranchDto branchDto)
        {
            var branch = _mapper.Map<Branch>(branchDto);
            branch.CreatedDate = DateTime.UtcNow;
            branch.CreatedBy = "system";
            
            var newBranch = await _branchRepository.AddAsync(branch);
            return ResponseDto<BranchDto>.CreateSuccessResponse(_mapper.Map<BranchDto>(newBranch), "Branch created successfully.");
        }

        public async Task<ResponseDto<bool>> DeleteBranchAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null) return ResponseDto<bool>.CreateErrorResponse("Branch not found.");
            
            await _branchRepository.DeleteAsync(branch);
            return ResponseDto<bool>.CreateSuccessResponse(true, "Branch deleted.");
        }
    }
}
