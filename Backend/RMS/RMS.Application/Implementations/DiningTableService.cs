using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.DTOs.DiningTables;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using RMS.Domain.Extensions;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.IRepositories;

namespace RMS.Application.Implementations
{
    public class DiningTableService : IDiningTableService
    {
        private readonly IDiningTableRepository _diningTableRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDiningTableDto> _createValidator;
        private readonly IValidator<UpdateDiningTableDto> _updateValidator;
        private readonly IValidator<UpdateDiningTableStatusDto> _updateStatusValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<DiningTableService> _logger;

        public DiningTableService(
            IDiningTableRepository diningTableRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateDiningTableDto> createValidator,
            IValidator<UpdateDiningTableDto> updateValidator,
            IValidator<UpdateDiningTableStatusDto> updateStatusValidator,
            IAuditLogService auditLogService,
            ILogger<DiningTableService> logger)
        {
            _diningTableRepository = diningTableRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _updateStatusValidator = updateStatusValidator;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task<ResponseDto<DiningTableDto>> GetDiningTableByIdAsync(int id)
        {
            var diningTable = await _diningTableRepository.GetByIdAsync(id);
            if (diningTable == null)
            {
                return ResponseDto<DiningTableDto>.CreateErrorResponse("Dining Table not found.", ApiErrorCode.NotFound);
            }
            return ResponseDto<DiningTableDto>.CreateSuccessResponse(_mapper.Map<DiningTableDto>(diningTable));
        }

        public async Task<ResponseDto<PagedResult<DiningTableDto>>> GetAllDiningTablesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection)
        {
            var query = _diningTableRepository.GetQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(dt => dt.TableName.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(dt => dt.TableName);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var diningTableDtos = _mapper.Map<List<DiningTableDto>>(pagedResult.Items);

            return ResponseDto<PagedResult<DiningTableDto>>.CreateSuccessResponse(new PagedResult<DiningTableDto>
            {
                Items = diningTableDtos,
                TotalRecords = pagedResult.TotalRecords,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            });
        }

        public async Task<ResponseDto<DiningTableDto>> CreateDiningTableAsync(CreateDiningTableDto diningTableDto)
        {
            var validationResult = await _createValidator.ValidateAsync(diningTableDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<DiningTableDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var diningTable = _mapper.Map<DiningTable>(diningTableDto);
            await _diningTableRepository.AddAsync(diningTable);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("CreateDiningTable", "DiningTable", $"TableId:{diningTable.TableID}", "System", $"Dining Table '{diningTable.TableName}' created.");

            return ResponseDto<DiningTableDto>.CreateSuccessResponse(_mapper.Map<DiningTableDto>(diningTable), "Dining Table created successfully.", "201");
        }

        public async Task<ResponseDto<DiningTableDto>> UpdateDiningTableAsync(int id, UpdateDiningTableDto diningTableDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(diningTableDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<DiningTableDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var existingDiningTable = await _diningTableRepository.GetByIdAsync(id);
            if (existingDiningTable == null)
            {
                return ResponseDto<DiningTableDto>.CreateErrorResponse("Dining Table not found.", ApiErrorCode.NotFound);
            }

            _mapper.Map(diningTableDto, existingDiningTable);
            await _diningTableRepository.UpdateAsync(existingDiningTable);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("UpdateDiningTable", "DiningTable", $"TableId:{existingDiningTable.TableID}", "System", $"Dining Table '{existingDiningTable.TableName}' updated.");

            return ResponseDto<DiningTableDto>.CreateSuccessResponse(_mapper.Map<DiningTableDto>(existingDiningTable), "Dining Table updated successfully.");
        }

        public async Task<ResponseDto<bool>> UpdateDiningTableStatusAsync(int id, bool status)
        {
            var validationResult = await _updateStatusValidator.ValidateAsync(new UpdateDiningTableStatusDto { TableID = id, Status = status });
            if (!validationResult.IsValid)
            {
                return ResponseDto<bool>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var diningTable = await _diningTableRepository.GetByIdAsync(id);
            if (diningTable == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Dining Table not found.", ApiErrorCode.NotFound);
            }

            diningTable.Status = status;

            await _diningTableRepository.UpdateAsync(diningTable);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("UpdateDiningTableStatus", "DiningTable", $"TableId:{id}", "System", $"Dining Table '{diningTable.TableName}' status updated to {(status ? "Active" : "Inactive")}.");

            return ResponseDto<bool>.CreateSuccessResponse(true, "Dining Table status updated successfully.");
        }

        public async Task<ResponseDto<bool>> DeleteDiningTableAsync(int id)
        {
            var diningTable = await _diningTableRepository.GetByIdAsync(id);
            if (diningTable == null)
            {
                return ResponseDto<bool>.CreateErrorResponse("Dining Table not found.", ApiErrorCode.NotFound);
            }

            await _diningTableRepository.DeleteAsync(diningTable);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.LogAsync("DeleteDiningTable", "DiningTable", $"TableId:{id}", "System", $"Dining Table '{diningTable.TableName}' deleted.");

            return ResponseDto<bool>.CreateSuccessResponse(true, "Dining Table deleted successfully.");
        }
    }
}