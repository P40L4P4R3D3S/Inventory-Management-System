using Inventory_Managment_System.Domain.Exceptions;
using System;

namespace Inventory_Managment_System.Domain.Models
{
    public class InventoryLot
    {
        public int Id { get; private set; }

        public string LotNumber { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public DateTime? ExpirationDate { get; private set; }

        public decimal UnitCost { get; private set; }

        public int InitialQuantity { get; private set; }

        public int QuantityOnHand { get; private set; }

        public string Supplier { get; private set; }

        public bool IsEmpty => QuantityOnHand == 0;

        public bool IsExpired =>
            ExpirationDate.HasValue &&
            ExpirationDate.Value.Date < DateTime.Today;

        public InventoryLot(
            string lotNumber,
            int quantity,
            decimal unitCost,
            DateTime receivedDate,
            DateTime? expirationDate = null,
            string supplier = "")
        {
            ValidateLotNumber(lotNumber);
            ValidatePositiveQuantity(quantity);
            ValidateUnitCost(unitCost);
            ValidateExpDate(expirationDate, receivedDate);

            LotNumber = lotNumber.Trim();
            InitialQuantity = quantity;
            QuantityOnHand = quantity;
            UnitCost = unitCost;
            ReceivedDate = receivedDate;
            ExpirationDate = expirationDate;
            Supplier = supplier?.Trim() ?? string.Empty;
        }

        public void AssignId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Lot ID must be greater than zero.");
            }

            if (Id != 0)
            {
                throw new InvalidOperationException(
                    "The lot already has an ID.");
            }

            Id = id;
        }

        public void Ship(int quantity)
        {
            ValidatePositiveQuantity(quantity);

            if (QuantityOnHand < quantity)
            {
                throw new LessInventoryException(
                    QuantityOnHand);
            }

            QuantityOnHand -= quantity;
        }

        private static void ValidatePositiveQuantity(
            int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity cannot be negative.");
            }
        }

        private static void ValidateLotNumber(string lotNumber)
        {
            if (string.IsNullOrWhiteSpace(lotNumber))
            {
                throw new ArgumentException(
                    "Lot number cannot be empty.",
                    nameof(lotNumber));
            }
        }

        private static void ValidateUnitCost(decimal unitCost)
        {
            if (unitCost < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCost),
                    "Unit cost cannot be negative.");
            }
        }

        private static void ValidateExpDate(DateTime? expirationDate, DateTime receivedDate)
        {
            if (expirationDate.HasValue &&
                expirationDate.Value.Date < receivedDate.Date)
            {
                throw new ArgumentException(
                    "Expiration date cannot be before the received date.",
                    nameof(expirationDate));
            }
        }
    }
}