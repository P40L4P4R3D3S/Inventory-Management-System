using Inventory_Managment_System.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory_Managment_System.Domain.Models
{
    public class Product
    {
        private readonly List<InventoryLot> _lots = [];
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string SKU { get; private set; }

        public IReadOnlyList<InventoryLot> Lots =>
            _lots.AsReadOnly();

        public int QuantityOnHand =>
            _lots.Sum(lot => lot.QuantityOnHand);

        public Product(
            string name,
            string description,
            decimal price,
            string sku)
        {
            ValidateName(name);
            ValidateSku(sku);
            ValidatePrice(price);

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Price = price;
            SKU = sku.Trim();
        }

        public void AssignId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Product ID must be greater than zero.");
            }

            if (Id != 0)
            {
                throw new InvalidOperationException(
                    "The product already has an ID.");
            }

            Id = id;
        }

        public void UpdatePrice(decimal price)
        {
            ValidatePrice(price);
            Price = price;
        }

        public void AddLot(InventoryLot lot)
        {
            ArgumentNullException.ThrowIfNull(lot);

            bool lotNumberExists = _lots.Any(existingLot =>
                existingLot.LotNumber.Equals(
                    lot.LotNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (lotNumberExists)
            {
                throw new InvalidOperationException(
                    $"Lot '{lot.LotNumber}' already exists " +
                    $"for product '{SKU}'.");
            }

            _lots.Add(lot);
        }

        public InventoryLot GetLotByNumber(
            string lotNumber)
        {
            if (string.IsNullOrWhiteSpace(lotNumber))
            {
                throw new ArgumentException(
                    "Lot number cannot be empty.",
                    nameof(lotNumber));
            }

            return _lots.FirstOrDefault(lot =>
                lot.LotNumber.Equals(
                    lotNumber.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                ?? throw new NotFoundException(
                    $"Lot '{lotNumber}' was not found.");
        }

        public void ShipFromLot(
            string lotNumber,
            int quantity)
        {
            InventoryLot lot =
                GetLotByNumber(lotNumber);

            lot.Ship(quantity);
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Product name cannot be empty.",
                    nameof(name));
            }
        }

        private static void ValidateSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException(
                    "SKU cannot be empty.",
                    nameof(sku));
            }
        }

        private static void ValidatePrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Price cannot be negative.");
            }
        }

        private static void ValidatePositiveQuantity(
            int quantity)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity cannot be negative.");
        }
    }
}