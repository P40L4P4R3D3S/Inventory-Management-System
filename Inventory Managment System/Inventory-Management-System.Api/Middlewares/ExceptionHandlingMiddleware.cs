using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Inventory_Management_System.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Inventory_Management_System.Api.Middlewares
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
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,

                DuplicateException => HttpStatusCode.Conflict,

                ArgumentException => HttpStatusCode.BadRequest,

                InvalidOperationException => HttpStatusCode.BadRequest,

                UnauthorizedAccessException => HttpStatusCode.Unauthorized,

                _ => HttpStatusCode.InternalServerError,
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "An unexpected error occurred.");
            }

            object response = new
            {
                status = (int)statusCode,
                message = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected server error occurred."
                    : exception.Message,
            };

            context.Response.StatusCode = (int)statusCode;

            context.Response.ContentType = "application/json";

            string json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
