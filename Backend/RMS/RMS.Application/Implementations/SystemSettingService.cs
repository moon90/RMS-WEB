using RMS.Infrastructure.IRepositories;
using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.DTOs.SystemSettings;
using RMS.Domain.Interfaces;
using RMS.Core.Enum;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMS.Application.Interfaces;

namespace RMS.Application.Implementations
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISystemSettingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuditLogService _auditLogService;

        public SystemSettingService(ISystemSettingRepository repository, IMapper mapper, IAuditLogService auditLogService)
        {
            _repository = repository;
            _mapper = mapper;
            _auditLogService = auditLogService;
        }

        public async Task<ResponseDto<IEnumerable<SystemSettingDto>>> GetAllSettingsAsync()
        {
            await SeedDefaultsAsync(); // Ensure defaults exist
            var settings = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<SystemSettingDto>>(settings);
            return ResponseDto<IEnumerable<SystemSettingDto>>.CreateSuccessResponse(dtos, "Settings retrieved successfully.");
        }

        private async Task SeedDefaultsAsync()
        {
            var existingSettings = await _repository.GetAllAsync();
            var existingKeys = existingSettings.Select(s => s.SettingKey).ToHashSet();

            var defaults = new List<SystemSetting>
            {
                new SystemSetting { SettingKey = "RestaurantName", SettingValue = "RMS Restaurant", SettingGroup = "General", SettingType = "String", Description = "Name of the restaurant" },
                new SystemSetting { SettingKey = "PageTitle", SettingValue = "RMS | Management System", SettingGroup = "General", SettingType = "String", Description = "Browser tab title" },
                new SystemSetting { SettingKey = "RestaurantAddress", SettingValue = "123 Food Street, Culinary City", SettingGroup = "General", SettingType = "String", Description = "Physical address" },
                new SystemSetting { SettingKey = "RestaurantPhone", SettingValue = "+1 234 567 890", SettingGroup = "General", SettingType = "String", Description = "Contact phone" },
                new SystemSetting { SettingKey = "RestaurantEmail", SettingValue = "info@restaurant.com", SettingGroup = "General", SettingType = "String", Description = "Contact email" },
                new SystemSetting { SettingKey = "VATNumber", SettingValue = "VAT123456", SettingGroup = "General", SettingType = "String", Description = "Tax registration number" },
                new SystemSetting { SettingKey = "CurrencySymbol", SettingValue = "৳", SettingGroup = "Localization", SettingType = "String", Description = "Currency symbol for prices" },
                new SystemSetting { SettingKey = "TimeZone", SettingValue = "Gulf Standard Time (GST)", SettingGroup = "Localization", SettingType = "String", Description = "System timezone" },
                new SystemSetting { SettingKey = "RestaurantLogo", SettingValue = "", SettingGroup = "Appearance", SettingType = "Image", Description = "Main restaurant logo" },
                new SystemSetting { SettingKey = "Favicon", SettingValue = "", SettingGroup = "Appearance", SettingType = "Image", Description = "Browser tab icon" },
                new SystemSetting { SettingKey = "PrimaryColor", SettingValue = "#1e40af", SettingGroup = "Appearance", SettingType = "Color", Description = "Main branding color" },
                new SystemSetting { SettingKey = "DefaultTaxRate", SettingValue = "5", SettingGroup = "POS", SettingType = "Number", Description = "Default VAT/Tax percentage" },
                new SystemSetting { SettingKey = "ServiceChargeRate", SettingValue = "10", SettingGroup = "POS", SettingType = "Number", Description = "Default service charge percentage" },
                new SystemSetting { SettingKey = "AutoPrintKOT", SettingValue = "false", SettingGroup = "POS", SettingType = "Boolean", Description = "Automatically print tickets to kitchen" },
                new SystemSetting { SettingKey = "AutoPrintReceipt", SettingValue = "true", SettingGroup = "POS", SettingType = "Boolean", Description = "Automatically print receipt after payment" },
                new SystemSetting { SettingKey = "ReceiptHeaderNote", SettingValue = "Welcome to Our Restaurant!", SettingGroup = "Receipt", SettingType = "String", Description = "Header note on printed receipts" },
                new SystemSetting { SettingKey = "ReceiptFooterNote", SettingValue = "Thank you for dining with us!", SettingGroup = "Receipt", SettingType = "String", Description = "Footer note on printed receipts" }
            };

            foreach (var item in defaults)
            {
                if (!existingKeys.Contains(item.SettingKey))
                {
                    await _repository.AddAsync(item);
                }
            }
        }

        public async Task<ResponseDto<SystemSettingDto>> GetSettingByKeyAsync(string key)
        {
            var setting = await _repository.GetByKeyAsync(key);
            if (setting == null)
                return ResponseDto<SystemSettingDto>.CreateErrorResponse($"Setting with key {key} not found.", ApiErrorCode.NotFound);

            var dto = _mapper.Map<SystemSettingDto>(setting);
            return ResponseDto<SystemSettingDto>.CreateSuccessResponse(dto, "Setting retrieved successfully.");
        }

        public async Task<ResponseDto<bool>> UpdateSettingAsync(UpdateSystemSettingDto updateDto)
        {
            var setting = await _repository.GetByKeyAsync(updateDto.SettingKey);
            if (setting == null)
                return ResponseDto<bool>.CreateErrorResponse($"Setting with key {updateDto.SettingKey} not found.", ApiErrorCode.NotFound);

            setting.SettingValue = updateDto.SettingValue;
            setting.ModifiedDate = DateTime.UtcNow;
            
            await _repository.UpdateAsync(setting);
            await _auditLogService.LogAsync("UPDATE", "SystemSetting", setting.SettingKey, "admin", $"Updated value to: {updateDto.SettingValue}");
            return ResponseDto<bool>.CreateSuccessResponse(true, "Setting updated successfully.");
        }

        public async Task<ResponseDto<bool>> UpdateSettingsAsync(IEnumerable<UpdateSystemSettingDto> updateDtos)
        {
            var settings = await _repository.GetAllAsync();
            var updatedSettings = new List<SystemSetting>();

            foreach (var updateDto in updateDtos)
            {
                var setting = settings.FirstOrDefault(s => s.SettingKey == updateDto.SettingKey);
                if (setting != null)
                {
                    setting.SettingValue = updateDto.SettingValue;
                    setting.ModifiedDate = DateTime.UtcNow;
                    updatedSettings.Add(setting);
                }
            }

            if (updatedSettings.Any())
            {
                await _repository.UpdateRangeAsync(updatedSettings);
                foreach (var s in updatedSettings)
                {
                    await _auditLogService.LogAsync("UPDATE_BULK", "SystemSetting", s.SettingKey, "admin", $"Bulk updated value to: {s.SettingValue}");
                }
            }

            return ResponseDto<bool>.CreateSuccessResponse(true, "Settings updated successfully.");
        }

        public async Task<ResponseDto<SystemSettingDto>> CreateSettingAsync(SystemSettingDto createDto)
        {
            var existing = await _repository.GetByKeyAsync(createDto.SettingKey);
            if (existing != null)
                return ResponseDto<SystemSettingDto>.CreateErrorResponse($"Setting with key {createDto.SettingKey} already exists.", ApiErrorCode.BadRequest);

            var setting = _mapper.Map<SystemSetting>(createDto);
            setting.CreatedDate = DateTime.UtcNow;
            setting.CreatedBy = "admin"; // Should come from identity

            await _repository.AddAsync(setting);
            await _auditLogService.LogAsync("CREATE", "SystemSetting", setting.SettingKey, "admin", $"Created setting with value: {setting.SettingValue}");
            var resultDto = _mapper.Map<SystemSettingDto>(setting);
            return ResponseDto<SystemSettingDto>.CreateSuccessResponse(resultDto, "Setting created successfully.");
        }

        public async Task<ResponseDto<bool>> DeleteSettingAsync(int id)
        {
            // Repository does not expose GetByIdAsync; delete directly.
            await _repository.DeleteAsync(id);
            // Log the deletion (key not retrieved).
            await _auditLogService.LogAsync("DELETE", "SystemSetting", $"Id:{id}", "admin", $"Deleted setting with Id {id}");
            return ResponseDto<bool>.CreateSuccessResponse(true, "Setting deleted successfully.");
        }
    }
}
