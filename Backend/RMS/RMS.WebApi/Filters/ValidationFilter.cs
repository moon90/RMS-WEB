using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using RMS.Application.DTOs;

namespace RMS.WebApi.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var validationErrors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => new ValidationDetailDto
                    {
                        PropertyName = x.Key,
                        ErrorMessage = e.ErrorMessage
                    }))
                    .ToList();

                context.Result = new BadRequestObjectResult(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Validation failed.",
                    Code = "VALIDATION_ERROR",
                    Details = validationErrors
                });

                return;
            }

            await next();
        }
    }
}
