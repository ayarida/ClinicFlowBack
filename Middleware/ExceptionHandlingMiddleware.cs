using System.Text.Json;
using ClinicFlow.Application.Common.Exceptions;
using FluentValidation;

namespace ClinicFlow.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (status, title, errors) = exception switch
        {
            ValidationException ve => (
                StatusCodes.Status400BadRequest,
                "Validation failed.",
                ve.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())),

            NotFoundException => (StatusCodes.Status404NotFound, exception.Message, (Dictionary<string, string[]>?)null),
            ConflictException => (StatusCodes.Status409Conflict, exception.Message, (Dictionary<string, string[]>?)null),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized.", (Dictionary<string, string[]>?)null),

            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", (Dictionary<string, string[]>?)null)
        };

        context.Response.StatusCode = status;

        var body = errors is not null
            ? new { title, status, errors }
            : (object)new { title, status };

        return context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }
}
