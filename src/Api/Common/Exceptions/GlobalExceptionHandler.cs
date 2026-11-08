using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Exceptions;

public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = exception switch
        {
            AppValidationException validationException =>
                new ValidationProblemDetails(validationException.Errors)
                {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                },

            ValidationException ex =>
                new ValidationProblemDetails(
                    ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()))
                        {
                            Title = "Validation failed",
                            Detail = "One or more validation errors occurred.",
                            Status = StatusCodes.Status400BadRequest
                        },

            BadRequestException ex => 
                new ProblemDetails
                {
                    Title = "BadRequest error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                },

            UnAuthorizeException ex => 
                new ProblemDetails
                {
                    Title = "UnAuthorize error access denied",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized
                },

            NotFoundException ex => 
                new ProblemDetails
                {
                    Title = "NotFound error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                },


            ConflictException ex => 
                new ProblemDetails
                {
                    Title = "Conflict error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                },
            
            
            DomainException ex => 
                new ProblemDetails
                {
                    Title = "Domain Rule error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                },

            _ => 
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError
                }
        };

        httpContext.Response.StatusCode = problemDetails.Status 
            ?? StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }
}