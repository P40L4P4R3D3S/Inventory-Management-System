using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.Models.Requests
{
    public class UpdateUserRoleRequest
    {
        public Role Role { get; set; }
    }
}
