using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Application.Interfaces
{
    public interface IInventoryService
    {
        void AddProduct(Product product);
        public void UpdateProduct(int id, decimal price);
        IReadOnlyList<Product> GetAllProducts();
        IReadOnlyList<Product> SearchProductsByName(string Name);
        Product GetProductBySku(string SKU);
        Product GetProductById(int id);
        InventoryLot ReceiveProduct(string sku, string lotNumber, int quantity, DateTime receivedDate, DateTime? expirationDate, string supplier);
        Product ShipProduct(string sku, int quantity);
    }
}
