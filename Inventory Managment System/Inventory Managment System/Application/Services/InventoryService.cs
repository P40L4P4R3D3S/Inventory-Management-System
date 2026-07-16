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
        private readonly List<Product> _products = [];
        private int _nextId = 1;

        public void AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            bool skuExists = _products.Any(existingProduct =>
                existingProduct.SKU.Equals(
                    product.SKU,
                    StringComparison.OrdinalIgnoreCase));

            if (skuExists)
            {
                throw new DuplicateSkuException(
                    product.SKU);
            }

            product.AssignId(_nextId++);
            _products.Add(product);
        }

        public IReadOnlyList<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        public void UpdateProduct(
            int id,
            decimal price,
            int quantity)
        {
            Product product = GetProductById(id);

            product.Update(price, quantity);
        }

        public IReadOnlyList<Product> SearchProductsByName(
            string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Product name cannot be empty.",
                    nameof(name));
            }

            string normalizedName = name.Trim();

            return _products
                .Where(product =>
                    product.Name.Contains(
                        normalizedName,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Product GetProductBySku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException(
                    "SKU cannot be empty.",
                    nameof(sku));
            }

            string normalizedSku = sku.Trim();
            return FindProduct(
                product => product.SKU.Equals(
                    normalizedSku,
                    StringComparison.OrdinalIgnoreCase),
                $"Product with SKU '{normalizedSku}' was not found.");
        }

        public Product GetProductById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Product ID must be greater than zero.");
            }
            return FindProduct(
                product => product.Id == id,
                $"Product with ID '{id}' was not found.");
        }

        public Product ReceiveProduct(
            string sku,
            int quantity)
        {
            Product product = GetProductBySku(sku);

            product.Receive(quantity);

            return product;
        }

        public Product ShipProduct(
            string sku,
            int quantity)
        {
            Product product = GetProductBySku(sku);

            product.Ship(quantity);

            return product;
        }

        private Product FindProduct(
            Func<Product, bool> condition,
            string errorMessage)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return _products.FirstOrDefault(condition)
                ?? throw new NotFoundException(errorMessage);
        }
    }
}