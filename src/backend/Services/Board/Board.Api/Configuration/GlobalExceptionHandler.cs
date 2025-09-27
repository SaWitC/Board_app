using System.Diagnostics;
using Board.Domain.Options;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Configuration;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (int statusCode, ProblemDetails problem) = CreateProblemDetails(httpContext, exception);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        await _problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem
        });
        return true;
    }

    private static (int statusCode, ProblemDetails problem) CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        ProblemDetails problemDetails;
        int statusCode;

        // FluentValidation exceptions (from FastEndpoints validators) are wrapped differently; handle typical cases
        if (exception is ValidationException validationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
            Dictionary<string, string[]> errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            problemDetails = new ValidationProblemDetails(errors)
            {
                Title = "Validation failed",
                Detail = "One or more validation errors occurred.",
                Status = statusCode,
                Type = "https://datatracker.ietf.org/doc/html/rfc7807#section-3.1",
                Instance = httpContext.Request.Path
            };
        }
        else if (exception is ForbiddenAccessException forbidden)
        {
            statusCode = StatusCodes.Status403Forbidden;
            problemDetails = new ProblemDetails
            {
                Title = "Access denied",
                Detail = string.IsNullOrWhiteSpace(forbidden.Message) ? "You do not have permission to perform this action." : forbidden.Message,
                Status = statusCode,
                Type = "https://httpstatuses.io/403",
                Instance = httpContext.Request.Path
            };
        }
        else if (exception is UnauthorizedAccessException unauthorized)
        {
            statusCode = StatusCodes.Status401Unauthorized;
            problemDetails = new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = string.IsNullOrWhiteSpace(unauthorized.Message) ? "Authentication is required to access this resource." : unauthorized.Message,
                Status = statusCode,
                Type = "https://httpstatuses.io/401",
                Instance = httpContext.Request.Path
            };
        }
        else if (exception is NotImplementedException notImplemented)
        {
            statusCode = StatusCodes.Status501NotImplemented;
            problemDetails = new ProblemDetails
            {
                Title = "Not Implemented",
                Detail = string.IsNullOrWhiteSpace(notImplemented.Message) ? "This functionality is not implemented." : notImplemented.Message,
                Status = statusCode,
                Type = "https://httpstatuses.io/501",
                Instance = httpContext.Request.Path
            };
        }
        else if (exception is OptionsException optionsException)
        {
            statusCode = StatusCodes.Status500InternalServerError;
            problemDetails = new ProblemDetails
            {
                Title = "Configuration error",
                Detail = optionsException.Message,
                Status = statusCode,
                Type = "https://httpstatuses.io/500",
                Instance = httpContext.Request.Path
            };
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred. Please try again later.",
                Status = statusCode,
                Type = "about:blank",
                Instance = httpContext.Request.Path
            };
        }

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        return (statusCode, problemDetails);
    }
}

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message) { }
}
