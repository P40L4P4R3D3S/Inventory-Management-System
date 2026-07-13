using Inventory_Managment_System.Application.Interfaces;
using Inventory_Managment_System.Domain.Exceptions;
using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory_Managment_System.Application.Services
{
    internal class InventoryService : IInventoryService
    {

        private readonly List<Product> _products = new();
        private int _nextId = 1;

        public void AddProduct(Product product)
        {
            bool skuExists = _products.Any(existingProduct =>
                existingProduct.SKU.Equals(
                    product.SKU,
                    StringComparison.OrdinalIgnoreCase));

            if (skuExists)
            {
                throw new DuplicateSkuException(product.SKU);
            }

            product.AssignId(_nextId);
            _nextId++;

            _products.Add(product);
        }

        public IReadOnlyList<Product> GetAllProducts()
        {
            return _products.AsReadOnly();
        }
    }
}
