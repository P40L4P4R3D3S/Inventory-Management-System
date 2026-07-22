using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Enums;

public class AuthenticationResponse
{
    public int UserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public Role Role { get; init; }

    public string Token { get; init; } = string.Empty;

    public static AuthenticationResponse FromResult(AuthenticationResult result)
    {
        return new()
        {
            UserId = result.UserId,
            Name = result.Name,
            Role = result.Role,
            Token = result.Token,
        };
    }
}
