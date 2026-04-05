using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using SharpPet.Application.Common.Exceptions;

namespace SharpPet.Api.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case NotFoundException nf:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new
                        {
                            title = "Resource not found",
                            detail = nf.Message,
                            entity = nf.EntityName,
                            key = nf.Key
                        },
                        JsonSerializerOptions.Web),
                    cancellationToken);
                return true;
            case ValidationException ve:
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = "application/json";
                var errors = ve.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                await httpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new { title = "Validation failed", errors },
                        JsonSerializerOptions.Web),
                    cancellationToken);
                return true;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new { title = "Unexpected error", detail = "An error occurred." },
                        JsonSerializerOptions.Web),
                    cancellationToken);
                return true;
        }
    }
}
