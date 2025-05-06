using RMS.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RMS.Domain.Models.BaseModels
{
    public class Result
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }

        [JsonIgnore]
        public ResultType Type { get; set; } = ResultType.Undefined;

        public Result()
        {
            Succeeded = true;
        }

        public Result(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        public Result(bool succeeded, string error, ResultType type)
        {
            Succeeded = succeeded;
            Error = error;
            Type = type;
        }

        public static Result Success()
            => new();

        public static Result Success(string message)
            => new(true, message);

        public static Result Failure(string error)
            => new(false, error);

        public static Result Failure(string error, ResultType type)
            => new(false, error, type);
    }

    public class Result<T> : Result where T : class
    {
        public T? Data { get; set; }

        public Result() : base() { }

        public Result(T data)
        {
            Data = data;
            Succeeded = true;
        }

        public Result(T data, string message)
        {
            Succeeded = true;
            Error = message;
            Data = data;
        }

        public Result(bool succeeded, string error) : base(succeeded, error) { }

        public Result(bool succeeded, string error, ResultType type) : base(succeeded, error, type) { }

        public static Result<T> Success(T data) => new(data);
        public static Result<T> Success(T data, string message) => new(data, message);

        public static new Result<T> Failure(string msg) => new(false, msg);
        public static new Result<T> Failure(string error, ResultType type)
            => new(false, error, type);
    }
}
