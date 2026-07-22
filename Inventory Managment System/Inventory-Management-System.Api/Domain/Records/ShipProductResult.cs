using System.Collections.Generic;

namespace Inventory_Management_System.Api.Domain.Records
{
    public sealed record ShipProductResult(
        int ProductId,
        int TotalShipped,
        IReadOnlyList<ShipmentTransactionResult> Transactions
    );
}
