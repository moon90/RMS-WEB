using AutoMapper;
using CsvHelper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using RMS.Application.DTOs.CategoryDTOs.OutputDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Extensions;
using RMS.Domain.Interfaces;
using RMS.Domain.Models.BaseModels;
using RMS.Infrastructure.IRepositories;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;

namespace RMS.Application.Implementations
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDto> _createValidator;
        private readonly IValidator<CategoryUpdateDto> _updateValidator;
        private readonly IAuditLogService _auditLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Category> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IValidator<CategoryCreateDto> createValidator,
            IValidator<CategoryUpdateDto> updateValidator,
            IAuditLogService auditLogService,
            IUnitOfWork unitOfWork,
            ILogger<Category> logger)
            : base(mapper, categoryRepository, unitOfWork, logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _auditLogService = auditLogService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ImportResultDto> ImportCategoriesAsync(IFormFile file)
        {
            var result = new ImportResultDto();
            if (file == null || file.Length == 0)
            {
                result.IsSuccess = false;
                result.Message = "File is empty.";
                return result;
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
            {
                result.IsSuccess = false;
                result.Message = "Invalid file format. Please upload a CSV file.";
                return result;
            }

            var records = new List<CategoryCreateDto>();
            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                records = csv.GetRecords<CategoryCreateDto>().ToList();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Error parsing CSV file: {ex.Message}";
                return result;
            }

            var validationErrors = new List<ValidationErrorDto>();
            var rowNumber = 2; // Assuming row 1 is header

            // Check for duplicates within the CSV file
            var duplicateNames = records.GroupBy(r => r.CategoryName.ToLower())
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key);

            if (duplicateNames.Any())
            {
                foreach (var name in duplicateNames)
                {
                    validationErrors.Add(new ValidationErrorDto
                    {
                        RowNumber = 0, // General error
                        PropertyName = "CategoryName",
                        ErrorMessage = $"Duplicate category name '{name}' found within the CSV file."
                    });
                }
            }


            foreach (var record in records)
            {
                var validationResult = await _createValidator.ValidateAsync(record);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        validationErrors.Add(new ValidationErrorDto
                        {
                            RowNumber = rowNumber,
                            PropertyName = error.PropertyName,
                            ErrorMessage = error.ErrorMessage
                        });
                    }
                }
                rowNumber++;
            }

            if (validationErrors.Count > 0)
            {
                result.IsSuccess = false;
                result.Message = "Validation failed for one or more records.";
                result.ValidationErrors = validationErrors;
                return result;
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var categoriesToAdd = _mapper.Map<List<Category>>(records);
                await _categoryRepository.AddRangeAsync(categoriesToAdd);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                result.IsSuccess = true;
                result.Message = $"{records.Count} categories imported successfully.";
                result.ImportedCount = records.Count;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error during bulk category import.");
                result.IsSuccess = false;
                result.Message = "An unexpected error occurred during the import process. The operation has been rolled back.";
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return new ResponseDto<IEnumerable<CategoryDto>>
                {
                    IsSuccess = true,
                    Message = "Categories retrieved successfully.",
                    Code = "200",
                    Data = categoryDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories.");
                return new ResponseDto<IEnumerable<CategoryDto>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving categories.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }

        public async Task<PagedResult<CategoryDto>> GetAllCategoriesAsync(int pageNumber, int pageSize, string? searchQuery, string? sortColumn, string? sortDirection, bool? status)
        {
            try
            {
                var query = _categoryRepository.GetQueryable();

                if (status.HasValue)
                {
                    query = query.Where(c => c.Status == status.Value);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(c => c.CategoryName.Contains(searchQuery));
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    query = query.ApplySort(sortColumn, sortDirection ?? "asc");
                }
                else
                {
                    query = query.OrderBy(c => c.CategoryName);
                }

                var pagedResult = await query.ToPagedList(pageNumber, pageSize);

                var categoryDtos = _mapper.Map<List<CategoryDto>>(pagedResult.Items);
                return new PagedResult<CategoryDto>(categoryDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllCategoriesAsync (paged).");
                throw;
            }
        }

        public async Task<FileExportDto> ExportCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            using var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(categoryDtos);
            }

            memoryStream.Position = 0;

            return new FileExportDto
            {
                Content = memoryStream.ToArray(),
                ContentType = "text/csv",
                FileName = $"categories_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv"
            };
        }

        public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryCreateDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var category = _mapper.Map<Category>(createDto);
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);

            await _auditLogService.LogAsync(
                action: "CreateCategory",
                entityType: "Category",
                entityId: $"CategoryId:{category.CategoryID}",
                performedBy: "System",
                details: $"Category '{category.CategoryName}' created."
            );

            return new ResponseDto<CategoryDto>
            {
                IsSuccess = true,
                Message = "Category created successfully.",
                Code = "201",
                Data = categoryDto
            };
        }

        public async Task<ResponseDto<CategoryDto>> UpdateAsync(CategoryUpdateDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "400",
                    Details = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(updateDto.CategoryID);
            if (existingCategory == null)
            {
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "Category not found.",
                    Code = "404"
                };
            }

            _mapper.Map(updateDto, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(existingCategory);

            await _auditLogService.LogAsync(
                action: "UpdateCategory",
                entityType: "Category",
                entityId: $"CategoryId:{existingCategory.CategoryID}",
                performedBy: "System",
                details: $"Category '{existingCategory.CategoryName}' updated."
            );

            return new ResponseDto<CategoryDto>
            {
                IsSuccess = true,
                Message = "Category updated successfully.",
                Code = "200",
                Data = categoryDto
            };
        }

        public async Task<ResponseDto<string>> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "Category not found.",
                    Code = "404"
                };
            }

            await _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "DeleteCategory",
                entityType: "Category",
                entityId: $"CategoryId:{id}",
                performedBy: "System",
                details: $"Category '{category.CategoryName}' deleted."
            );

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Category deleted successfully.",
                Code = "200",
                Data = $"CategoryId:{id}"
            };
        }

        public async Task<ResponseDto<string>> UpdateStatusAsync(int id, bool status)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "Category not found.",
                    Code = "404"
                };
            }

            category.Status = status;
            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "UpdateCategoryStatus",
                entityType: "Category",
                entityId: $"CategoryId:{id}",
                performedBy: "System",
                details: $"Category '{category.CategoryName}' status updated to {(status ? "Active" : "Inactive")}."
            );

            return new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "Category status updated successfully.",
                Code = "200",
                Data = $"CategoryId:{id}"
            };
        }

        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return new ResponseDto<CategoryDto>
                    {
                        IsSuccess = false,
                        Message = "Category not found.",
                        Code = "404"
                    };
                }
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = true,
                    Message = "Category retrieved successfully.",
                    Code = "200",
                    Data = categoryDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by ID.");
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the category.",
                    Code = "500",
                    Details = ex.Message
                };
            }
        }
    }
}
