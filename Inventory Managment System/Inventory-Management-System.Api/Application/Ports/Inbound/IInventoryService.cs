using System;
using System.Collections.Generic;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Application.Ports.Inbound
{
    public interface IInventoryService
    {
        void AddProduct(Product product);
        IReadOnlyList<Product> GetAllProducts();
        IReadOnlyList<Product> SearchProductsByName(string name);
        Product GetProductById(int id);
        Product GetProductBySku(string sku);
        void UpdateProduct(int id, decimal? price, string? name, string? description);
        void DeleteProduct(int id);

        InventoryLot ReceiveProduct(
            string sku,
            string lotNumber,
            int quantity,
            DateTime receivedDate,
            DateTime? expirationDate,
            string supplier
        );

        InventoryLot ShipProduct(string sku, string lotNumber, int quantity);

        InventoryLot GetLot(string sku, string lotNumber);

        IReadOnlyList<InventoryLot> GetProductLots(string sku);
    }
}
