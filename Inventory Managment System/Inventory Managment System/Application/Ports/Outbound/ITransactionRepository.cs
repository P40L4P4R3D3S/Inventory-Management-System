using Inventory_Managment_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Application.Ports.Outbound
{
    public interface ITransactionRepository
    {
        IReadOnlyList<InventoryTransaction> GetAll();
        void SaveAll(IEnumerable<InventoryTransaction> transactions);
    }
}
