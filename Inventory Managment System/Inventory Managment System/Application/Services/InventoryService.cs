using Inventory_Managment_System.Application.Interfaces;
using Inventory_Managment_System.Domain.Exceptions;
using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void UpdateProduct(int id, decimal price, int quantity)
        {
            Product productToUpdate = SearchProductsById(id); 

            Product product = new(
                productToUpdate.Name,
                productToUpdate.Description,
                price,
                productToUpdate.SKU,
                quantity);
            _products.RemoveAll(p => p.Id == id);
            product.AssignId(productToUpdate.Id);
            _products.Add(product);
        }

        public IReadOnlyList<Product> SearchProductsByName(string name)
        {
            return _products
                .Where(product =>
                    product.Name.Contains(
                        name,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Product? SearchProductsBySKU(string sku)
        {
            Product? product = _products.FirstOrDefault(product =>
                product.SKU.Equals(
                    sku,
                    StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                throw new NotFoundException();
            }

            return product;
        }
        public Product? SearchProductsById(int id)
        {
            Product? product = _products.FirstOrDefault(product =>
                product.Id == id);
            if (product == null)
            {
                throw new NotFoundException();
            }

            return product;
        }
        public Product? ReceiveProduct(string sku, int quantity)
        {
            if(quantity <= 0)
            {
                throw new GreaterThanZeroException();
            }
            Product p = SearchProductsBySKU(sku);
            p.QuantityOnHand += quantity;
            return p;
        }
        public Product? ShipProduct(string sku, int quantity)
        {
            if (quantity <= 0)
            {
                throw new GreaterThanZeroException();
            }

            Product p = SearchProductsBySKU(sku);
            if (p.QuantityOnHand < quantity)
            {
                throw new LessInventoryException(p.QuantityOnHand);
            }

            p.QuantityOnHand -= quantity;
            return p;
        }
    }
}