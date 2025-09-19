using RMS.Core.Enum;

namespace RMS.Application.DTOs
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }           // Indicates if the response is successful
        public string? Message { get; set; }           // General message (success or error)
        public string? Code { get; set; }              // Status code or error code
        public  T? Data { get; set; }                   // Payload (for success)
        public object? Details { get; set; }           // Additional error details (for failure)

        public static ResponseDto<T> CreateSuccessResponse(T data, string message = "Success", string code = "200")
        {
            return new ResponseDto<T>
            {
                IsSuccess = true,
                Message = message,
                Code = code,
                Data = data
            };
        }

        public static ResponseDto<T> CreateErrorResponse(string message, ApiErrorCode errorCode = ApiErrorCode.ServerError, object? details = null)
        {
            return new ResponseDto<T>
            {
                IsSuccess = false,
                Message = message,
                Code = ((int)errorCode).ToString(),
                Data = default(T),
                Details = details
            };
        }
    }
}
