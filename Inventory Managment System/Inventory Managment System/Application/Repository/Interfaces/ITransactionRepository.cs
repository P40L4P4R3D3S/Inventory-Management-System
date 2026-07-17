using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Application.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        IReadOnlyList<InventoryTransaction> GetAll();
        void SaveAll(
            IEnumerable<InventoryTransaction> transactions);
    }
}
