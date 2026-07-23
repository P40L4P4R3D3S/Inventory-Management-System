using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Inventory_Management_System.Api.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Inventory_Management_System.Api.API.Middlewares
{
    public class CurrentUserMiddleware
    {
        public const string CurrentUserKey = "CurrentUser";

        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ClaimsPrincipal principal = context.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                string? idClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);

                string? nameClaim = principal.FindFirstValue(ClaimTypes.Name);

                string? roleClaim = principal.FindFirstValue(ClaimTypes.Role);

                bool validId = int.TryParse(idClaim, out int userId);

                bool validRole = Enum.TryParse(roleClaim, ignoreCase: true, out Role role);

                if (validId && validRole)
                {
                    context.Items[CurrentUserKey] = new CurrentUser(
                        userId,
                        nameClaim ?? string.Empty,
                        role
                    );
                }
            }

            await _next(context);
        }
    }
}
