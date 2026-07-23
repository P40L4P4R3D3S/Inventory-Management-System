using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.API.Middlewares
{
    public sealed record CurrentUser(int Id, string Name, Role Role);
}
