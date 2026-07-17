using Inventory_Managment_System.Application.Repository;
using Inventory_Managment_System.Application.Repository.Interfaces;
using Inventory_Managment_System.Domain.Models;
using System.Collections.Generic;

namespace Inventory_Managment_System.Data.Repositories
{
    public class TransactionRepository
        : ITransactionRepository
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