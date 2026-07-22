using System.Text.Json.Serialization;

namespace Inventory_Management_System.Api.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<Role>))]
    public enum Role
    {
        Admin,
        Manager,
        Warehouse,
        Viewer,
    }
}
