using System;
using Inventory_Management_System.Api.API.Middlewares;
using Microsoft.AspNetCore.Http;

namespace Inventory_Management_System.Api.API.extensions
{
    public static class CurrentUserHttpContextExtensions
    {
        public static CurrentUser GetCurrentUser(this HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (
                context.Items.TryGetValue(CurrentUserMiddleware.CurrentUserKey, out object? value)
                && value is CurrentUser currentUser
            )
            {
                return currentUser;
            }

            throw new UnauthorizedAccessException(
                "The authenticated user could not be identified."
            );
        }
    }
}
