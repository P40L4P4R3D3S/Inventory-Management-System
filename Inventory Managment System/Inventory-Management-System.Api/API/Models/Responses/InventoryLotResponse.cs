using System;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.API.Models.Responses
{
    public class InventoryLotResponse
    {
        public int Id { get; init; }

        public string LotNumber { get; init; } = string.Empty;

        public int ReceivedQuantity { get; init; }

        public int QuantityOnHand { get; init; }

        public decimal UnitCost { get; init; }

        public DateTime ReceivedDate { get; init; }

        public DateTime? ExpirationDate { get; init; }

        public string Supplier { get; init; } = string.Empty;

        public bool IsEmpty { get; init; }

        public bool IsExpired { get; init; }

        public static InventoryLotResponse FromDomain(InventoryLot lot)
        {
            return new InventoryLotResponse
            {
                Id = lot.Id,
                LotNumber = lot.LotNumber,
                ReceivedQuantity = lot.InitialQuantity,
                QuantityOnHand = lot.QuantityOnHand,
                UnitCost = lot.UnitCost,
                ReceivedDate = lot.ReceivedDate,
                ExpirationDate = lot.ExpirationDate,
                Supplier = lot.Supplier,
                IsEmpty = lot.IsEmpty,
                IsExpired = lot.IsExpired,
            };
        }
    }
}
