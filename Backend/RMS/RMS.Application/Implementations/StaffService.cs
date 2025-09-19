using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.StaffDTOs.InputDTOs;
using RMS.Application.DTOs.StaffDTOs.OutputDTOs;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Extensions;
using System.Linq;

namespace RMS.Application.Implementations
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateStaffDto> _createValidator;
        private readonly IValidator<UpdateStaffDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StaffService> _logger;

        public StaffService(
            IStaffRepository staffRepository,
            IMapper mapper,
            IValidator<CreateStaffDto> createValidator,
            IValidator<UpdateStaffDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<StaffService> logger)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<StaffDto>> GetByIdAsync(int id)
        {
            var query = _staffRepository.GetQueryable();
            var staff = await query.FirstOrDefaultAsync(s => s.StaffID == id);

            if (staff == null)
            {
                return new ResponseDto<StaffDto> { IsSuccess = false, Message = "Staff not found.", Code = "404" };
            }
            var staffDto = _mapper.Map<StaffDto>(staff);
            return new ResponseDto<StaffDto> { IsSuccess = true, Data = staffDto, Code = "200" };
        }

        public async Task<PagedResult<StaffDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _staffRepository.GetQueryable();

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(s => s.StaffName.Contains(searchQuery) || s.StaffPhone.Contains(searchQuery) || s.StaffRole.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(s => s.StaffName); // Default sort
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var staffDtos = _mapper.Map<List<StaffDto>>(pagedResult.Items);
            return new PagedResult<StaffDto>(staffDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<StaffDto>> CreateAsync(CreateStaffDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Staff creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<StaffDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var staff = _mapper.Map<Staff>(createDto);
            await _staffRepository.AddAsync(staff);
            await _unitOfWork.SaveChangesAsync();

            var staffDto = _mapper.Map<StaffDto>(staff);
            await _auditLogService.LogAsync("CreateStaff", "Staff", $"StaffId:{staff.StaffID}", "System", $"Staff '{staff.StaffName}' created.");

            return new ResponseDto<StaffDto> { IsSuccess = true, Message = "Staff created successfully.", Data = staffDto, Code = "201" };
        }

        public async Task<ResponseDto<StaffDto>> UpdateAsync(UpdateStaffDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<StaffDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingStaff = await _staffRepository.GetByIdAsync(updateDto.StaffID);
            if (existingStaff == null)
            {
                return new ResponseDto<StaffDto> { IsSuccess = false, Message = "Staff not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingStaff);
            await _staffRepository.UpdateAsync(existingStaff);
            await _unitOfWork.SaveChangesAsync();

            var staffDto = _mapper.Map<StaffDto>(existingStaff);
            await _auditLogService.LogAsync("UpdateStaff", "Staff", $"StaffId:{existingStaff.StaffID}", "System", $"Staff '{existingStaff.StaffName}' updated.");

            return new ResponseDto<StaffDto> { IsSuccess = true, Message = "Staff updated successfully.", Data = staffDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Staff not found.", Code = "404" };
            }

            await _staffRepository.DeleteAsync(staff);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteStaff", "Staff", $"StaffId:{id}", "System", $"Staff '{staff.StaffName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Staff deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Staff not found.", Code = "404" };
            }

            staff.Status = status;
            await _staffRepository.UpdateAsync(staff);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateStaffStatus", "Staff", $"StaffId:{id}", "System", $"Staff '{staff.StaffName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Staff status updated successfully.", Code = "200" };
        }
    }
}

