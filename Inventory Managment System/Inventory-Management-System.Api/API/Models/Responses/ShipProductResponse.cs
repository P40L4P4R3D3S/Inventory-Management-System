using System.Collections.Generic;
using System.Linq;
using Inventory_Management_System.Api.Domain.Records;

namespace Inventory_Management_System.Api.API.Models.Responses
{
    public class ShipProductResponse
    {
        public int ProductId { get; init; }

        public int TotalShipped { get; init; }

        public IReadOnlyList<ShipmentTransactionResponse> Transactions { get; init; } = [];

        public static ShipProductResponse FromApplication(ShipProductResult result)
        {
            return new ShipProductResponse
            {
                ProductId = result.ProductId,
                TotalShipped = result.TotalShipped,
                Transactions = result
                    .Transactions.Select(ShipmentTransactionResponse.FromApplication)
                    .ToList(),
            };
        }
    }
}
