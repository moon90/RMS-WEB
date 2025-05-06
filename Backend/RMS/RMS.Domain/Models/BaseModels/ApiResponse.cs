using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.BaseModels
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public ApiErrorCode ErrorCode { get; set; }

        public ApiResponse(bool success, string message, T data, ApiErrorCode errorCode = ApiErrorCode.None)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Create Success Response with data type is T
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns>ApiResponse with status true and message if any</returns>
        public static ApiResponse<T> CreateSuccessResponse(T data, string message = "Success", ApiErrorCode errorCode = ApiErrorCode.None)
        {
            return new ApiResponse<T>(true, message, data, errorCode);
        }

        /// <summary>
        /// Create Error Response with data type is default(T)
        /// </summary>
        /// <param name="message"></param>
        /// <returns>ApiResponse with status false and message if any</returns>
        public static ApiResponse<T> CreateErrorResponse(string message, ApiErrorCode errorCode = ApiErrorCode.ServerError)
        {
            return new ApiResponse<T>(false, message, default, errorCode);
        }
    }
}
