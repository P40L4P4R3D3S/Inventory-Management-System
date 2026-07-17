using System;
using System.Collections.Generic;

using Inventory_Managment_System.Domain.Entities;
using Inventory_Managment_System.Domain.Models;

namespace Inventory_Managment_System.Application.Ports.Inbound
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