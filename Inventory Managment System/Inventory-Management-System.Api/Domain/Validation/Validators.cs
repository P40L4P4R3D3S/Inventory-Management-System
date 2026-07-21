using System;

namespace Inventory_Management_System.Api.Domain.Validation
{
    public static class Validators
    {
        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }
        }

        public static void ValidateSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("SKU cannot be empty.", nameof(sku));
            }
        }

        public static void ValidatePrice(decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            }
        }

        public static void ValidatePositiveQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity cannot be negative."
                );
            }
        }

        public static void ValidateLotNumber(string lotNumber)
        {
            if (string.IsNullOrWhiteSpace(lotNumber))
            {
                throw new ArgumentException("Lot number cannot be empty.", nameof(lotNumber));
            }
        }

        public static void ValidateUnitCost(decimal unitCost)
        {
            if (unitCost < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCost),
                    "Unit cost cannot be negative."
                );
            }
        }

        public static void ValidateExpDate(DateTime? expirationDate, DateTime receivedDate)
        {
            if (expirationDate.HasValue && expirationDate.Value.Date < receivedDate.Date)
            {
                throw new ArgumentException(
                    "Expiration date cannot be before the received date.",
                    nameof(expirationDate)
                );
            }
        }

        public static void ValidatePagination(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageNumber),
                    "Page number must be greater than zero."
                );
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageSize),
                    "Page size must be greater than zero."
                );
            }

            if (pageSize > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageSize),
                    "Page size cannot be greater than 100."
                );
            }
        }
    }
}
