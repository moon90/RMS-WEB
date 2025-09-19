using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs.CustomerDTOs.InputDTOs;
using RMS.Application.DTOs.CustomerDTOs.OutputDTOs;
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

namespace RMS.Application.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<CustomerDto>> GetByIdAsync(int id)
        {
            var query = _customerRepository.GetQueryable();
            var customer = await query.FirstOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null)
            {
                return new ResponseDto<CustomerDto> { IsSuccess = false, Message = "Customer not found.", Code = "404" };
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return new ResponseDto<CustomerDto> { IsSuccess = true, Data = customerDto, Code = "200" };
        }

        public async Task<PagedResult<CustomerDto>> GetAllAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            var query = _customerRepository.GetQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.CustomerName.Contains(searchQuery) || c.CustomerPhone.Contains(searchQuery) || c.CustomerEmail.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.ApplySort(sortColumn, sortDirection ?? "asc");
            }
            else
            {
                query = query.OrderBy(c => c.CustomerName);
            }

            var pagedResult = await query.ToPagedList(pageNumber, pageSize);
            var customerDtos = _mapper.Map<List<CustomerDto>>(pagedResult.Items);
            return new PagedResult<CustomerDto>(customerDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<ResponseDto<CustomerDto>> CreateAsync(CreateCustomerDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                _logger.LogError("Customer creation validation failed. Details: {ErrorDetails}", System.Text.Json.JsonSerializer.Serialize(errorDetails));
                return new ResponseDto<CustomerDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = errorDetails };
            }

            var customer = _mapper.Map<Customer>(createDto);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            var customerDto = _mapper.Map<CustomerDto>(customer);
            await _auditLogService.LogAsync("CreateCustomer", "Customer", $"CustomerId:{customer.CustomerID}", "System", $"Customer '{customer.CustomerName}' created.");

            return new ResponseDto<CustomerDto> { IsSuccess = true, Message = "Customer created successfully.", Data = customerDto, Code = "201" };
        }

        public async Task<ResponseDto<CustomerDto>> UpdateAsync(UpdateCustomerDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<CustomerDto> { IsSuccess = false, Message = "Validation failed.", Code = "400", Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
            }

            var existingCustomer = await _customerRepository.GetByIdAsync(updateDto.CustomerID);
            if (existingCustomer == null)
            {
                return new ResponseDto<CustomerDto> { IsSuccess = false, Message = "Customer not found.", Code = "404" };
            }

            _mapper.Map(updateDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);
            await _unitOfWork.SaveChangesAsync();

            var customerDto = _mapper.Map<CustomerDto>(existingCustomer);
            await _auditLogService.LogAsync("UpdateCustomer", "Customer", $"CustomerId:{existingCustomer.CustomerID}", "System", $"Customer '{existingCustomer.CustomerName}' updated.");

            return new ResponseDto<CustomerDto> { IsSuccess = true, Message = "Customer updated successfully.", Data = customerDto, Code = "200" };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Customer not found.", Code = "404" };
            }

            await _customerRepository.DeleteAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("DeleteCustomer", "Customer", $"CustomerId:{id}", "System", $"Customer '{customer.CustomerName}' deleted.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Customer deleted successfully.", Code = "200" };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return new ResponseDto<string> { IsSuccess = false, Message = "Customer not found.", Code = "404" };
            }

            customer.Status = status;
            await _customerRepository.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("UpdateCustomerStatus", "Customer", $"CustomerId:{id}", "System", $"Customer '{customer.CustomerName}' status updated to {(status ? "Active" : "Inactive")}.");

            return new ResponseDto<string> { IsSuccess = true, Message = "Customer status updated successfully.", Code = "200" };
        }
    }
}
