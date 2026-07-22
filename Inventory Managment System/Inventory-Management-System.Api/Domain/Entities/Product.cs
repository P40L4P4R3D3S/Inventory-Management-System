using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Inventory_Management_System.Api.Domain.Exceptions;
using Inventory_Management_System.Api.Domain.Validation;

namespace Inventory_Management_System.Api.Domain.Entities
{
    public class Product
    {
        [JsonInclude]
        public int Id { get; private set; }

        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude]
        public string Description { get; private set; }

        [JsonInclude]
        public decimal Price { get; private set; }

        [JsonInclude]
        public string SKU { get; private set; }

        [JsonInclude]
        [JsonPropertyName("Lots")]
        private List<InventoryLot> StoredLots { get; set; } = [];

        [JsonIgnore]
        public IReadOnlyList<InventoryLot> Lots => StoredLots.AsReadOnly();

        [JsonIgnore]
        public int QuantityOnHand => StoredLots.Sum(lot => lot.QuantityOnHand);

        public Product(string name, string description, decimal price, string sku)
        {
            Validators.ValidateName(name);
            Validators.ValidateSku(sku);
            Validators.ValidatePrice(price);

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
                    "Product ID must be greater than zero."
                );
            }

            if (Id != 0)
            {
                throw new InvalidOperationException("The product already has an ID.");
            }

            Id = id;
        }

        public void UpdatePrice(decimal price)
        {
            Validators.ValidatePrice(price);

            Price = price;
        }

        public void UpdateName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Validators.ValidateName(name);

                Name = name.Trim();
            }
        }

        public void UpdateDescription(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                Description = description.Trim();
            }
        }

        public void AddLot(InventoryLot lot)
        {
            ArgumentNullException.ThrowIfNull(lot);

            bool lotNumberExists = StoredLots.Any(existingLot =>
                existingLot.LotNumber.Equals(lot.LotNumber, StringComparison.OrdinalIgnoreCase)
            );

            if (lotNumberExists)
            {
                throw new InvalidOperationException(
                    $"Lot '{lot.LotNumber}' already exists " + $"for product '{SKU}'."
                );
            }

            StoredLots.Add(lot);
        }

        public InventoryLot GetLotByNumber(string lotNumber)
        {
            if (string.IsNullOrWhiteSpace(lotNumber))
            {
                throw new ArgumentException("Lot number cannot be empty.", nameof(lotNumber));
            }

            string normalizedLotNumber = lotNumber.Trim();

            return StoredLots.FirstOrDefault(lot =>
                    lot.LotNumber.Equals(normalizedLotNumber, StringComparison.OrdinalIgnoreCase)
                )
                ?? throw new NotFoundException(
                    $"Lot '{normalizedLotNumber}' was not found " + $"for product '{SKU}'."
                );
        }

        public InventoryLot ShipFromLot(string lotNumber, int quantity)
        {
            InventoryLot lot = GetLotByNumber(lotNumber);

            lot.Ship(quantity);

            return lot;
        }
    }
}
