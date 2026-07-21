using System;
using System.Collections.Generic;
using System.Linq;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Enums;
using Inventory_Management_System.Api.Domain.Exceptions;
using Inventory_Management_System.Api.Domain.Validation;

namespace Inventory_Management_System.Api.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly List<Product> _products;
        private readonly List<InventoryTransaction> _transactions;

        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;

        private int _nextProductId;
        private int _nextLotId;
        private int _nextTransactionId;

        public InventoryService(
            IProductRepository productRepository,
            ITransactionRepository transactionRepository
        )
        {
            _productRepository =
                productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _transactionRepository =
                transactionRepository
                ?? throw new ArgumentNullException(nameof(transactionRepository));

            _products = _productRepository.GetAll().ToList();
            _transactions = _transactionRepository.GetAll().ToList();
            _nextProductId = GetNextProductId();
            _nextLotId = GetNextLotId();
            _nextTransactionId = GetNextTransactionId();
        }

        private int GetNextLotId()
        {
            int maximumLotId = _products
                .SelectMany(product => product.Lots)
                .Select(lot => lot.Id)
                .DefaultIfEmpty(0)
                .Max();

            return maximumLotId + 1;
        }

        private int GetNextProductId()
        {
            return _products.Select(product => product.Id).DefaultIfEmpty(0).Max() + 1;
        }

        private int GetNextTransactionId()
        {
            return _transactions.Select(transaction => transaction.Id).DefaultIfEmpty(0).Max() + 1;
        }

        public void AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            bool nameExists = _products.Any(existingProduct =>
                existingProduct.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)
            );

            if (nameExists)
            {
                throw new DuplicateException("name", product.Name);
            }

            bool skuExists = _products.Any(existingProduct =>
                existingProduct.SKU.Equals(product.SKU, StringComparison.OrdinalIgnoreCase)
            );

            if (skuExists)
            {
                throw new DuplicateException("SKU", product.SKU);
            }

            product.AssignId(_nextProductId++);
            _products.Add(product);

            _productRepository.SaveAll(_products);
        }

        public IReadOnlyList<Product> GetAllProducts()
        {
            return _products.OrderBy(p => p.Price).ToList();
        }

        public IReadOnlyList<Product> GetAllProducts(int pageNumber, int pageSize)
        {
            Validators.ValidatePagination(pageNumber, pageSize);

            return _products
                .OrderBy(product => product.Price)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetProductsCount(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return _products.Count;
            }

            string normalizedName = name.Trim();

            return _products.Count(product =>
                product.Name.Contains(normalizedName, StringComparison.OrdinalIgnoreCase)
            );
        }

        public Product GetProductById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Product ID must be greater than zero."
                );
            }

            return FindProduct(
                product => product.Id == id,
                $"Product with ID '{id}' was not found."
            );
        }

        public Product GetProductBySku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("SKU cannot be empty.", nameof(sku));
            }

            string normalizedSku = sku.Trim();

            return FindProduct(
                product => product.SKU.Equals(normalizedSku, StringComparison.OrdinalIgnoreCase),
                $"Product with SKU '{normalizedSku}' was not found."
            );
        }

        public IReadOnlyList<Product> SearchProductsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }

            string normalizedName = name.Trim();

            return _products
                .Where(product =>
                    product.Name.Contains(normalizedName, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();
        }

        public IReadOnlyList<Product> SearchProductsByName(
            string name,
            int pageNumber,
            int pageSize
        )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }

            Validators.ValidatePagination(pageNumber, pageSize);

            string normalizedName = name.Trim();

            return _products
                .Where(product =>
                    product.Name.Contains(normalizedName, StringComparison.OrdinalIgnoreCase)
                )
                .OrderBy(product => product.Price)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void UpdateProduct(int id, decimal? price, string? name, string? description)
        {
            Product product = GetProductById(id);

            if (price.HasValue)
            {
                product.UpdatePrice(price.Value);
            }

            product.UpdateName(name);
            product.UpdateDescription(description);

            _productRepository.SaveAll(_products);
        }

        public void DeleteProduct(int id)
        {
            Product product = GetProductById(id);
            _products.Remove(product);
            _productRepository.SaveAll(_products);
        }

        public InventoryLot ReceiveProduct(
            string sku,
            string lotNumber,
            int quantity,
            DateTime receivedDate,
            DateTime? expirationDate,
            string supplier
        )
        {
            Product product = GetProductBySku(sku);

            InventoryLot lot = new(
                lotNumber,
                quantity,
                product.Price,
                receivedDate,
                expirationDate,
                supplier
            );

            lot.AssignId(_nextLotId++);

            product.AddLot(lot);

            InventoryTransaction transaction = new(
                _nextTransactionId++,
                product.Id,
                lot.Id,
                TransactionType.Receive,
                quantity,
                DateTime.Now
            );

            _transactions.Add(transaction);

            _productRepository.SaveAll(_products);
            _transactionRepository.SaveAll(_transactions);

            return lot;
        }

        public InventoryLot GetLot(string sku, string lotNumber)
        {
            Product product = GetProductBySku(sku);

            return product.GetLotByNumber(lotNumber);
        }

        public IReadOnlyList<InventoryLot> GetProductLots(string sku)
        {
            Product product = GetProductBySku(sku);

            return product.Lots;
        }

        private Product FindProduct(Func<Product, bool> condition, string errorMessage)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return _products.FirstOrDefault(condition) ?? throw new NotFoundException(errorMessage);
        }

        public InventoryLot ShipProduct(string sku, string lotNumber, int quantity)
        {
            Product product = GetProductBySku(sku);

            InventoryLot lot = product.ShipFromLot(lotNumber, quantity);

            InventoryTransaction transaction = new(
                _nextTransactionId++,
                product.Id,
                lot.Id,
                TransactionType.Ship,
                quantity,
                DateTime.Now
            );

            _transactions.Add(transaction);

            _productRepository.SaveAll(_products);
            _transactionRepository.SaveAll(_transactions);

            return lot;
        }
    }
}
