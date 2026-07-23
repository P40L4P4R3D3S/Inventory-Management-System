using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.API.Models.Requests
{
    public class RegisterUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Viewer;
    }
}
