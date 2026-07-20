using System.Collections.Generic;

using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Application.Ports.Outbound
{
    public interface ITransactionRepository
    {
        IReadOnlyList<InventoryTransaction> GetAll();
        void SaveAll(IEnumerable<InventoryTransaction> transactions);
    }
}
