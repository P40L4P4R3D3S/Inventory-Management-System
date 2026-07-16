using Inventory_Managment_System.Domain.Exceptions;
using System;

namespace Inventory_Managment_System.Domain.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string SKU { get; private set; }
        public int QuantityOnHand { get; private set; }

        public Product(
            string name,
            string description,
            decimal price,
            string sku,
            int quantityOnHand)
        {
            ValidateName(name);
            ValidateSku(sku);
            ValidatePrice(price);
            ValidateQuantity(quantityOnHand);

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            SKU = sku.Trim();
            Price = price;
            QuantityOnHand = quantityOnHand;
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

        public void Update(decimal price, int quantity)
        {
            ValidatePrice(price);
            ValidateQuantity(quantity);

            Price = price;
            QuantityOnHand = quantity;
        }

        public void Receive(int quantity)
        {
            ValidateQuantity(quantity);

            QuantityOnHand += quantity;
        }

        public void Ship(int quantity)
        {
            ValidateQuantity(quantity);

            if (QuantityOnHand < quantity)
            {
                throw new LessInventoryException(
                    QuantityOnHand);
            }

            QuantityOnHand -= quantity;
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

        private static void ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity cannot be negative.");
            }
        }
    }
}