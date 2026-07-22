using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Application.Ports.Inbound
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
