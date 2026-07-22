using System;
using System.Text.Json.Serialization;
using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.Domain.Entities
{
    public class InventoryTransaction
    {
        [JsonInclude]
        public int Id { get; private set; }

        [JsonInclude]
        public int ProductId { get; private set; }

        [JsonInclude]
        public int LotId { get; private set; }

        [JsonInclude]
        public TransactionType Type { get; private set; }

        [JsonInclude]
        public int Quantity { get; private set; }

        [JsonInclude]
        public DateTime TransactionDate { get; private set; }

        [JsonInclude]
        public string Notes { get; private set; }

        public InventoryTransaction(
            int id,
            int productId,
            int lotId,
            TransactionType type,
            int quantity,
            DateTime transactionDate,
            string? notes = null
        )
        {
            Id = id;
            ProductId = productId;
            LotId = lotId;
            Type = type;
            Quantity = quantity;
            TransactionDate = transactionDate;
            Notes = notes?.Trim() ?? string.Empty;
        }
    }
}
