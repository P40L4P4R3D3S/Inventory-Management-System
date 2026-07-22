using System;
using Inventory_Management_System.Api.Domain.Records;

namespace Inventory_Management_System.Api.Models.Responses
{
    public class ShipmentTransactionResponse
    {
        public int TransactionId { get; init; }

        public int LotId { get; init; }

        public string LotNumber { get; init; } = string.Empty;

        public int QuantityShipped { get; init; }

        public DateTime? ExpirationDate { get; init; }

        public DateTime ReceivedDate { get; init; }

        public static ShipmentTransactionResponse FromApplication(ShipmentTransactionResult result)
        {
            return new ShipmentTransactionResponse
            {
                TransactionId = result.TransactionId,
                LotId = result.LotId,
                LotNumber = result.LotNumber,
                QuantityShipped = result.QuantityShipped,
                ExpirationDate = result.ExpirationDate,
                ReceivedDate = result.ReceivedDate,
            };
        }
    }
}
