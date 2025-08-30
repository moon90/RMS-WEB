
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.UnitDTOs.InputDTOs;
using RMS.Application.DTOs.UnitDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Dtos;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Domain.Queries;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUnitDto> _createValidator;
        private readonly IValidator<UpdateUnitDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnitService> _logger;

        public UnitService(
            IUnitRepository unitRepository,
            IMapper mapper,
            IValidator<CreateUnitDto> createValidator,
            IValidator<UpdateUnitDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<UnitService> logger)
        {
            _unitRepository = unitRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<UnitDto>> GetByIdAsync(int id)
        {
            var unitResult = await _unitRepository.GetByIdAsync(id);
            if (!unitResult.Succeeded || unitResult.Data == null)
            {
                return new ResponseDto<UnitDto> { IsSuccess = false, Message = "Unit not found.", Code = "404" };
            }
            var unitDto = _mapper.Map<UnitDto>(unitResult.Data);
            return new ResponseDto<UnitDto> { IsSuccess = true, Data = unitDto, Code = "200" };
        }

        public async Task<PagedResult<UnitDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _unitRepository.GetQueryable();

            if (status.HasValue)
            {
                query = query.Where(u => u.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(u => u.Name.Contains(searchQuery) || u.ShortCode.Contains(searchQuery));
            }

            var pagedResult = await _unitRepository.GetPagedResultAsync(new PagedQuery { PageNumber = pageNumber, PageSize = pageSize }, null, false, query);
            var unitDtos = _mapper.Map<List<UnitDto>>(pagedResult.Items);
            return new PagedResult<UnitDto>(unitDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<UnitDto>> CreateAsync(CreateUnitDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<UnitDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var unit = _mapper.Map<Unit>(createDto);
            await _unitRepository.AddAsync(unit);
            await _unitOfWork.SaveChangesAsync();

            var unitDto = _mapper.Map<UnitDto>(unit);
            await _auditLogService.LogAsync("CreateUnit", "Unit", $"UnitId:{unit.Id}", "System", $"Unit '{unit.Name}' created.");

            return new ResponseDto<UnitDto> { IsSuccess = true, Message = "Unit created successfully.", Data = unitDto, Code = "201" };
        }

        public async Task<ResponseDto<UnitDto>> UpdateAsync(UpdateUnitDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<UnitDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingUnitResult = await _unitRepository.GetByIdAsync(updateDto.Id);
            if (!existingUnitResult.Succeeded || existingUnitResult.Data == null)
            {
                return new ResponseDto<UnitDto> { IsSuccess = false, Message = "Unit not found.", Code = "404" };
            }

            var existingUnit = existingUnitResult.Data;
            _mapper.Map(updateDto, existingUnit);
            var updateResult = await _unitRepository.UpdateAsync(existingUnit);
            if (!updateResult.Succeeded)
            {
                return new ResponseDto<UnitDto>
                {
                    IsSuccess = false,
                    Message = updateResult.Error,
                    Code = "500"
                };
            }
            await _unitOfWork.SaveChangesAsync();

            var unitDto = _mapper.Map<UnitDto>(existingUnit);
            await _auditLogService.LogAsync("UpdateUnit", "Unit", $"UnitId:{existingUnit.Id}", "System", $"Unit '{existingUnit.Name}' updated.");

            return new ResponseDto<UnitDto> { IsSuccess = true, Message = "Unit updated successfully.", Data = unitDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var unitResult = await _unitRepository.GetByIdAsync(id);
            if (!unitResult.Succeeded || unitResult.Data == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Unit not found.", Code = "404" };
            }
            var unit = unitResult.Data;

            var deleteResult = await _unitRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();

            if (!deleteResult.Succeeded)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = deleteResult.Error,
                    Code = "500"
                };
            }

            await _auditLogService.LogAsync("DeleteUnit", "Unit", $"UnitId:{id}", "System", $"Unit '{unit.Name}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Unit deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var unitResult = await _unitRepository.GetByIdAsync(id);
            if (!unitResult.Succeeded || unitResult.Data == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Unit not found.", Code = "404" };
            }
            var unit = unitResult.Data;

            unit.Status = status;
            var updateResult = await _unitRepository.UpdateAsync(unit);
            await _unitOfWork.SaveChangesAsync();

            if (!updateResult.Succeeded)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = updateResult.Error,
                    Code = "500"
                };
            }

            await _auditLogService.LogAsync("UpdateUnitStatus", "Unit", $"UnitId:{id}", "System", $"Unit '{unit.Name}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Unit status updated successfully.", Code = "200" };
        }
    }
}
