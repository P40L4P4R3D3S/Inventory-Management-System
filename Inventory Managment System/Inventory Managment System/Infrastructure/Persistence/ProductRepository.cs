using System.Collections.Generic;

using Inventory_Managment_System.Application.Ports.Outbound;
using Inventory_Managment_System.Domain.Models;

namespace Inventory_Managment_System.Infrastructure.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly JsonRepository<Product> _repository;

        public ProductRepository(string filePath)
        {
            _repository =
                new JsonRepository<Product>(
                    filePath);
        }

        public IReadOnlyList<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public void SaveAll(
            IEnumerable<Product> products)
        {
            _repository.SaveAll(products);
        }
    }
}