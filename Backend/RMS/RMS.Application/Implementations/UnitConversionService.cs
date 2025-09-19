
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Implementations
{
    public class UnitConversionService : IUnitConversionService
    {
        private readonly IUnitConversionRepository _unitConversionRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUnitConversionDto> _createUnitConversionValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnitConversionService> _logger;

        public UnitConversionService(
            IUnitConversionRepository unitConversionRepository,
            IUnitRepository unitRepository,
            IMapper mapper,
            IValidator<CreateUnitConversionDto> createUnitConversionValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<UnitConversionService> logger)
        {
            _unitConversionRepository = unitConversionRepository;
            _unitRepository = unitRepository;
            _mapper = mapper;
            _createUnitConversionValidator = createUnitConversionValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<UnitConversionDto>> CreateUnitConversionAsync(CreateUnitConversionDto unitConversionDto)
        {
            var validationResult = await _createUnitConversionValidator.ValidateAsync(unitConversionDto);
            if (!validationResult.IsValid)
            {
                return ResponseDto<UnitConversionDto>.CreateErrorResponse("Validation failed.", ApiErrorCode.BadRequest, validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            // Check if units exist
            var fromUnit = await _unitRepository.GetByIdAsync(unitConversionDto.FromUnitID);
            var toUnit = await _unitRepository.GetByIdAsync(unitConversionDto.ToUnitID);

            if (fromUnit == null || toUnit == null)
            {
                return ResponseDto<UnitConversionDto>.CreateErrorResponse("FromUnit or ToUnit not found.", ApiErrorCode.NotFound);
            }

            // Check for duplicate conversion (e.g., A to B and B to A might be handled by one entry)
            var existingConversion = await _unitConversionRepository.GetQueryable()
                .FirstOrDefaultAsync(uc =>
                    (uc.FromUnitID == unitConversionDto.FromUnitID && uc.ToUnitID == unitConversionDto.ToUnitID) ||
                    (uc.FromUnitID == unitConversionDto.ToUnitID && uc.ToUnitID == unitConversionDto.FromUnitID));

            if (existingConversion != null)
            {
                return ResponseDto<UnitConversionDto>.CreateErrorResponse("A conversion for these units already exists.", ApiErrorCode.Conflict);
            }

            var unitConversion = _mapper.Map<UnitConversion>(unitConversionDto);
            var newUnitConversion = await _unitConversionRepository.AddAsync(unitConversion);
            await _unitOfWork.SaveChangesAsync();

            var unitConversionDtoResult = _mapper.Map<UnitConversionDto>(newUnitConversion);
            unitConversionDtoResult.FromUnitName = fromUnit.Name;
            unitConversionDtoResult.ToUnitName = toUnit.Name;

            await _auditLogService.LogAsync("CreateUnitConversion", "UnitConversion", $"UnitConversionId:{newUnitConversion.UnitConversionID}", "System", $"Unit conversion from {fromUnit.Name} to {toUnit.Name} created.");

            return ResponseDto<UnitConversionDto>.CreateSuccessResponse(unitConversionDtoResult, "Unit conversion created successfully.", "201");
        }

        public async Task<ResponseDto<List<UnitConversionDto>>> GetAllUnitConversionsAsync()
        {
            var unitConversions = await _unitConversionRepository.GetQueryable()
                .Include(uc => uc.FromUnit)
                .Include(uc => uc.ToUnit)
                .ToListAsync();

            var unitConversionDtos = _mapper.Map<List<UnitConversionDto>>(unitConversions);
            foreach (var dto in unitConversionDtos)
            {
                var originalConversion = unitConversions.FirstOrDefault(uc => uc.UnitConversionID == dto.UnitConversionID);
                if (originalConversion != null)
                {
                    dto.FromUnitName = originalConversion.FromUnit?.Name;
                    dto.ToUnitName = originalConversion.ToUnit?.Name;
                }
            }
            return ResponseDto<List<UnitConversionDto>>.CreateSuccessResponse(unitConversionDtos);
        }

        public async Task<ResponseDto<UnitConversionDto>> GetUnitConversionByIdAsync(int id)
        {
            var unitConversion = await _unitConversionRepository.GetQueryable()
                .Include(uc => uc.FromUnit)
                .Include(uc => uc.ToUnit)
                .FirstOrDefaultAsync(uc => uc.UnitConversionID == id);

            if (unitConversion == null)
            {
                return ResponseDto<UnitConversionDto>.CreateErrorResponse("Unit conversion not found.", ApiErrorCode.NotFound);
            }

            var unitConversionDto = _mapper.Map<UnitConversionDto>(unitConversion);
            unitConversionDto.FromUnitName = unitConversion.FromUnit?.Name;
            unitConversionDto.ToUnitName = unitConversion.ToUnit?.Name;

            return ResponseDto<UnitConversionDto>.CreateSuccessResponse(unitConversionDto);
        }

        public async Task<ResponseDto<decimal>> ConvertUnitsAsync(int fromUnitId, int toUnitId, decimal value)
        {
            if (fromUnitId == toUnitId)
            {
                return ResponseDto<decimal>.CreateSuccessResponse(value, "Units are the same, no conversion needed.", "200");
            }

            // Try direct conversion
            var directConversion = await _unitConversionRepository.GetQueryable()
                .FirstOrDefaultAsync(uc => uc.FromUnitID == fromUnitId && uc.ToUnitID == toUnitId);

            if (directConversion != null)
            {
                return ResponseDto<decimal>.CreateSuccessResponse(value * directConversion.ConversionFactor, "Conversion successful.", "200");
            }

            // Try reverse conversion (e.g., if A to B exists, use 1/factor for B to A)
            var reverseConversion = await _unitConversionRepository.GetQueryable()
                .FirstOrDefaultAsync(uc => uc.FromUnitID == toUnitId && uc.ToUnitID == fromUnitId);

            if (reverseConversion != null)
            {
                if (reverseConversion.ConversionFactor == 0)
                {
                    return ResponseDto<decimal>.CreateErrorResponse("Cannot convert due to zero conversion factor.", ApiErrorCode.BadRequest);
                }
                return ResponseDto<decimal>.CreateSuccessResponse(value / reverseConversion.ConversionFactor, "Conversion successful (reverse).", "200");
            }

            // TODO: Implement more complex conversions (e.g., A to C via B)
            // This would require a graph traversal algorithm.

            return ResponseDto<decimal>.CreateErrorResponse("No direct or reverse conversion found for the specified units.", ApiErrorCode.NotFound);
        }
    }
}
