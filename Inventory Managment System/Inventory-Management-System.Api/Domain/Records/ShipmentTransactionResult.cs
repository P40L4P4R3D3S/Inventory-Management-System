using System;

namespace Inventory_Management_System.Api.Domain.Records
{
    public record ShipmentTransactionResult(
        int TransactionId,
        int LotId,
        string LotNumber,
        int QuantityShipped,
        DateTime? ExpirationDate,
        DateTime ReceivedDate
    );
}
