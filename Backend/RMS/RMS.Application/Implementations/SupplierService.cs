
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.SupplierDTOs.InputDTOs;
using RMS.Application.DTOs.SupplierDTOs.OutputDTOs;
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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSupplierDto> _createValidator;
        private readonly IValidator<UpdateSupplierDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(
            ISupplierRepository supplierRepository,
            IMapper mapper,
            IValidator<CreateSupplierDto> createValidator,
            IValidator<UpdateSupplierDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<SupplierService> logger)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<SupplierDto>> GetByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                return new ResponseDto<SupplierDto> { IsSuccess = false, Message = "Supplier not found.", Code = "404" };
            }
            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            return new ResponseDto<SupplierDto> { IsSuccess = true, Data = supplierDto, Code = "200" };
        }

        public async Task<PagedResult<SupplierDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _supplierRepository.GetQueryable();

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(s => s.SupplierName.Contains(searchQuery) || s.ContactPerson.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(s => s.SupplierName); // Default sort
            }

            var pagedResult = await _supplierRepository.GetPagedResultAsync(new PagedQuery { PageNumber = pageNumber, PageSize = pageSize }, null, false, query);
            var supplierDtos = _mapper.Map<List<SupplierDto>>(pagedResult.Items);
            return new PagedResult<SupplierDto>(supplierDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<SupplierDto>> CreateAsync(CreateSupplierDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<SupplierDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var supplier = _mapper.Map<Supplier>(createDto);
            await _supplierRepository.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            await _auditLogService.LogAsync("CreateSupplier", "Supplier", $"SupplierId:{supplier.Id}", "System", $"Supplier '{supplier.SupplierName}' created.");

            return new ResponseDto<SupplierDto> { IsSuccess = true, Message = "Supplier created successfully.", Data = supplierDto, Code = "201" };
        }

        public async Task<ResponseDto<SupplierDto>> UpdateAsync(UpdateSupplierDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<SupplierDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingSupplier = await _supplierRepository.GetByIdAsync(updateDto.Id);
            if (existingSupplier == null)
            {
                return new ResponseDto<SupplierDto> { IsSuccess = false, Message = "Supplier not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingSupplier);
            await _supplierRepository.UpdateAsync(existingSupplier);
            await _unitOfWork.SaveChangesAsync();

            var supplierDto = _mapper.Map<SupplierDto>(existingSupplier);
            await _auditLogService.LogAsync("UpdateSupplier", "Supplier", $"SupplierId:{existingSupplier.Id}", "System", $"Supplier '{existingSupplier.SupplierName}' updated.");

            return new ResponseDto<SupplierDto> { IsSuccess = true, Message = "Supplier updated successfully.", Data = supplierDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Supplier not found.", Code = "404" };
            }

            await _supplierRepository.DeleteAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteSupplier", "Supplier", $"SupplierId:{id}", "System", $"Supplier '{supplier.SupplierName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Supplier deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Supplier not found.", Code = "404" };
            }

            supplier.Status = status;
            await _supplierRepository.UpdateAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateSupplierStatus", "Supplier", $"SupplierId:{id}", "System", $"Supplier '{supplier.SupplierName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Supplier status updated successfully.", Code = "200" };
        }
    }
}
