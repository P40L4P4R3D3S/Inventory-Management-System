using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;

namespace Inventory_Managment_System.Application.Interfaces
{
    public interface IInventoryService
    {
        void AddProduct(Product product);
        IReadOnlyList<Product> GetAllProducts();
        IReadOnlyList<Product> SearchProductsByName(string name);
        Product GetProductById(int id);
        Product GetProductBySku(string sku);
        void UpdateProduct(int id, decimal price);

        InventoryLot ReceiveProduct(
            string sku,
            string lotNumber,
            int quantity,
            DateTime receivedDate,
            DateTime? expirationDate,
            string supplier);

        InventoryLot ShipProduct(
            string sku,
            string lotNumber,
            int quantity);

        InventoryLot GetLot(
            string sku,
            string lotNumber);

        IReadOnlyList<InventoryLot> GetProductLots(
            string sku);
    }
}