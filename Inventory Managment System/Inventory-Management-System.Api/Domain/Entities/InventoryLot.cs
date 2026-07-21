using System;
using System.Text.Json.Serialization;
using Inventory_Management_System.Api.Domain.Exceptions;
using Inventory_Management_System.Api.Domain.Validation;

namespace Inventory_Management_System.Api.Domain.Entities
{
    public class InventoryLot
    {
        [JsonInclude]
        public int Id { get; private set; }

        [JsonInclude]
        public string LotNumber { get; private set; }

        [JsonInclude]
        public DateTime ReceivedDate { get; private set; }

        [JsonInclude]
        public DateTime? ExpirationDate { get; private set; }

        [JsonInclude]
        public decimal UnitCost { get; private set; }

        [JsonInclude]
        public int InitialQuantity { get; private set; }

        [JsonInclude]
        public int QuantityOnHand { get; private set; }

        [JsonInclude]
        public string Supplier { get; private set; }

        [JsonIgnore]
        public bool IsEmpty => QuantityOnHand == 0;

        [JsonIgnore]
        public bool IsExpired =>
            ExpirationDate.HasValue && ExpirationDate.Value.Date < DateTime.Today;

        public InventoryLot(
            string lotNumber,
            int quantity,
            decimal unitCost,
            DateTime receivedDate,
            DateTime? expirationDate = null,
            string supplier = ""
        )
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
                    "Lot ID must be greater than zero."
                );
            }

            if (Id != 0)
            {
                throw new InvalidOperationException("The lot already has an ID.");
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
                throw new LessInventoryException(QuantityOnHand);
            }

            QuantityOnHand -= quantity;
        }
    }
}
