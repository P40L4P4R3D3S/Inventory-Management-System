using System.Text.Json.Serialization;

namespace Inventory_Management_System.Api.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<TransactionType>))]
    public enum TransactionType
    {
        Receive,
        Ship,
    }
}
