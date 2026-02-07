using System.Collections.Generic;

namespace RMS.Application.DTOs;

public class ImportResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int ImportedCount { get; set; }
    public List<ValidationErrorDto> ValidationErrors { get; set; } = new List<ValidationErrorDto>();
}

public class ValidationErrorDto
{
    public int RowNumber { get; set; }
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}
