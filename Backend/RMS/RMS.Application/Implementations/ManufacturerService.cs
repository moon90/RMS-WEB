
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.ManufacturerDTOs.InputDTOs;
using RMS.Application.DTOs.ManufacturerDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
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
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateManufacturerDto> _createValidator;
        private readonly IValidator<UpdateManufacturerDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ManufacturerService> _logger;

        public ManufacturerService(
            IManufacturerRepository manufacturerRepository,
            IMapper mapper,
            IValidator<CreateManufacturerDto> createValidator,
            IValidator<UpdateManufacturerDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<ManufacturerService> logger)
        {
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<ManufacturerDto>> GetByIdAsync(int id)
        {
            var query = _manufacturerRepository.GetQueryable();
            var manufacturer = await query.FirstOrDefaultAsync(m => m.Id == id);

            if (manufacturer == null)
            {
                return new ResponseDto<ManufacturerDto> { IsSuccess = false, Message = "Manufacturer not found.", Code = "404" };
            }
            var manufacturerDto = _mapper.Map<ManufacturerDto>(manufacturer);
            return new ResponseDto<ManufacturerDto> { IsSuccess = true, Data = manufacturerDto, Code = "200" };
        }

        public async Task<PagedResult<ManufacturerDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _manufacturerRepository.GetQueryable();

            if (status.HasValue)
            {
                query = query.Where(m => m.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(m => m.ManufacturerName.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(m => m.ManufacturerName);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var manufacturerDtos = _mapper.Map<List<ManufacturerDto>>(pagedResult.Items);
            return new PagedResult<ManufacturerDto>(manufacturerDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<ManufacturerDto>> CreateAsync(CreateManufacturerDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<ManufacturerDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var manufacturer = _mapper.Map<Manufacturer>(createDto);
            await _manufacturerRepository.AddAsync(manufacturer);
            await _unitOfWork.SaveChangesAsync();

            var manufacturerDto = _mapper.Map<ManufacturerDto>(manufacturer);
            await _auditLogService.LogAsync("CreateManufacturer", "Manufacturer", $"ManufacturerId:{manufacturer.Id}", "System", $"Manufacturer '{manufacturer.ManufacturerName}' created.");

            return new ResponseDto<ManufacturerDto> { IsSuccess = true, Message = "Manufacturer created successfully.", Data = manufacturerDto, Code = "201" };
        }

        public async Task<ResponseDto<ManufacturerDto>> UpdateAsync(UpdateManufacturerDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<ManufacturerDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingManufacturer = await _manufacturerRepository.GetByIdAsync(updateDto.Id);
            if (existingManufacturer == null)
            {
                return new ResponseDto<ManufacturerDto> { IsSuccess = false, Message = "Manufacturer not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingManufacturer);
            await _manufacturerRepository.UpdateAsync(existingManufacturer);
            await _unitOfWork.SaveChangesAsync();

            var manufacturerDto = _mapper.Map<ManufacturerDto>(existingManufacturer);
            await _auditLogService.LogAsync("UpdateManufacturer", "Manufacturer", $"ManufacturerId:{existingManufacturer.Id}", "System", $"Manufacturer '{existingManufacturer.ManufacturerName}' updated.");

            return new ResponseDto<ManufacturerDto> { IsSuccess = true, Message = "Manufacturer updated successfully.", Data = manufacturerDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id);
            if (manufacturer == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Manufacturer not found.", Code = "404" };
            }

            await _manufacturerRepository.DeleteAsync(manufacturer);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteManufacturer", "Manufacturer", $"ManufacturerId:{id}", "System", $"Manufacturer '{manufacturer.ManufacturerName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Manufacturer deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id);
            if (manufacturer == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Manufacturer not found.", Code = "404" };
            }

            manufacturer.Status = status;
            await _manufacturerRepository.UpdateAsync(manufacturer);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateManufacturerStatus", "Manufacturer", $"ManufacturerId:{id}", "System", $"Manufacturer '{manufacturer.ManufacturerName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Manufacturer status updated successfully.", Code = "200" };
        }
    }
}
