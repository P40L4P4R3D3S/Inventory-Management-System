using System.Collections.Generic;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.Application.Ports.Inbound
{
    public interface IUserService
    {
        AuthenticationResult Register(
            string name,
            string password,
            Role role,
            int? performedByUserId = null
        );

        AuthenticationResult Login(string name, string password);

        User GetUserById(int id);

        IReadOnlyList<User> GetAllUsers(int authenticatedUserId);

        User UpdateCurrentUser(int authenticatedUserId, string? name, string? password);

        User UpdateUserRole(int authenticatedAdminId, int targetUserId, Role role);

        void DeleteCurrentUser(int authenticatedUserId);
    }

    public sealed record AuthenticationResult(int UserId, string Name, Role Role, string Token);
}
