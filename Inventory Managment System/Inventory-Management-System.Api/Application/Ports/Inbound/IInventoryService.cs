using System;
using System.Collections.Generic;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Records;

namespace Inventory_Management_System.Api.Application.Ports.Inbound
{
    public interface IInventoryService
    {
        void AddProduct(Product product);
        IReadOnlyList<Product> GetAllProducts();
        IReadOnlyList<Product> GetAllProducts(int pageNumber, int pageSize);
        IReadOnlyList<Product> SearchProductsByName(string name, int pageNumber, int pageSize);
        IReadOnlyList<Product> SearchProductsByName(string name);
        Product GetProductById(int id);
        Product GetProductBySku(string sku);
        void UpdateProduct(int id, decimal? price, string? name, string? description);
        void DeleteProduct(int id);
        int GetProductsCount(string? name);

        InventoryLot ReceiveProduct(
            string sku,
            string lotNumber,
            int quantity,
            DateTime receivedDate,
            DateTime? expirationDate,
            string supplier
        );

        ShipProductResult ShipProduct(int productId, int quantity, int? lotId, string? notes);

        InventoryLot GetLot(string sku, string lotNumber);

        IReadOnlyList<InventoryLot> GetProductLots(string sku);
    }
}
