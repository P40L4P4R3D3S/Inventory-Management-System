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
            Validators.ValidateLotNumber(lotNumber);
            Validators.ValidatePositiveQuantity(quantity);
            Validators.ValidateUnitCost(unitCost);
            Validators.ValidateExpDate(expirationDate, receivedDate);

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
            if (quantity <= 0)
            {
                Validators.ValidatePositiveQuantity(quantity);
            }

            if (QuantityOnHand < quantity)
            {
                throw new LessInventoryException(
                    QuantityOnHand);
            }

            QuantityOnHand -= quantity;
        }
    }
}