using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Inventory_Management_System.Api.Middlewares
{
    public class AuthorizationResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationResponseMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.HasStarted)
            {
                return;
            }

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await WriteResponseAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "A valid authentication token is required."
                );
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await WriteResponseAsync(
                    context,
                    StatusCodes.Status403Forbidden,
                    "You do not have permission to perform this operation."
                );
            }
        }

        private static async Task WriteResponseAsync(
            HttpContext context,
            int statusCode,
            string message
        )
        {
            context.Response.ContentType = "application/json";

            object response = new { status = statusCode, message };

            string json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
