using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Enums;

public class UserResponse
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public Role Role { get; init; }

    public static UserResponse FromDomain(User user)
    {
        return new()
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
        };
    }
}
