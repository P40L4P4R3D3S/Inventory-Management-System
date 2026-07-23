using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Inventory_Management_System.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Inventory_Management_System.Api.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger
        )
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogError(exception, "The response had already started.");

                    throw;
                }

                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = GetStatusCode(exception);

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "An unexpected server error occurred.");
            }
            else
            {
                _logger.LogWarning(
                    exception,
                    "Request failed with status code {StatusCode}.",
                    (int)statusCode
                );
            }

            context.Response.Clear();

            context.Response.StatusCode = (int)statusCode;

            context.Response.ContentType = "application/json";

            ErrorResponse response = new()
            {
                Status = (int)statusCode,
                Message =
                    statusCode == HttpStatusCode.InternalServerError
                        ? "An unexpected server error occurred."
                        : exception.Message,
            };

            string json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,

                DuplicateException => HttpStatusCode.Conflict,

                InventoryConflictException => HttpStatusCode.Conflict,

                LessInventoryException => HttpStatusCode.Conflict,

                ArgumentException => HttpStatusCode.BadRequest,

                InvalidOperationException => HttpStatusCode.BadRequest,

                UnauthorizedAccessException => HttpStatusCode.Unauthorized,

                _ => HttpStatusCode.InternalServerError,
            };
        }

        private sealed class ErrorResponse
        {
            public int Status { get; init; }

            public string Message { get; init; } = string.Empty;
        }
    }
}
