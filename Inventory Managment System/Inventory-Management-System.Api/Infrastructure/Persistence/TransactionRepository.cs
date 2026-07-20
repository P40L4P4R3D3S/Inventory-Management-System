using System.Collections.Generic;

using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Infrastructure.Persistence
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly JsonRepository<InventoryTransaction>
            _repository;

        public TransactionRepository(string filePath)
        {
            _repository =
                new JsonRepository<InventoryTransaction>(
                    filePath);
        }

        public IReadOnlyList<InventoryTransaction> GetAll()
        {
            return _repository.GetAll();
        }

        public void SaveAll(
            IEnumerable<InventoryTransaction> transactions)
        {
            _repository.SaveAll(transactions);
        }
    }
}
