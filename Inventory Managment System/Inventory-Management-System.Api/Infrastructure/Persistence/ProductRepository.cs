using System.Collections.Generic;

using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Infrastructure.Persistence
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
