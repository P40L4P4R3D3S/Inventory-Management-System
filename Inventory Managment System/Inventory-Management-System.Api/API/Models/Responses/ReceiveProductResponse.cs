using System;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.API.Models.Responses
{
    public class ReceiveProductResponse
    {
        public string Message { get; init; } = "Inventory received successfully.";

        public int ProductId { get; init; }

        public string ProductName { get; init; } = string.Empty;

        public string Sku { get; init; } = string.Empty;

        public InventoryLotResponse Lot { get; init; } = new();

        public int ProductQuantityOnHand { get; init; }

        public static ReceiveProductResponse FromDomain(Product product, InventoryLot lot)
        {
            return new ReceiveProductResponse
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Sku = product.SKU,
                Lot = InventoryLotResponse.FromDomain(lot),
                ProductQuantityOnHand = product.QuantityOnHand,
            };
        }
    }
}
