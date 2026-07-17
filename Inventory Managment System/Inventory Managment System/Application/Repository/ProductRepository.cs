using Inventory_Managment_System.Application.Repository.Interfaces;
using Inventory_Managment_System.Domain.Models;
using System.Collections.Generic;

namespace Inventory_Managment_System.Data.Repositories
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