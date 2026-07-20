using System;

using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.Domain.Entities
{
    public class InventoryTransaction
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int LotId { get; private set; }
        public TransactionType Type { get; private set; }
        public int Quantity { get; private set; }
        public DateTime TransactionDate { get; private set; }

        public InventoryTransaction(
            int id,
            int productId,
            int lotId,
            TransactionType type,
            int quantity,
            DateTime transactionDate)
        {
            Id = id;
            ProductId = productId;
            LotId = lotId;
            Type = type;
            Quantity = quantity;
            TransactionDate = transactionDate;
        }
    }
}
