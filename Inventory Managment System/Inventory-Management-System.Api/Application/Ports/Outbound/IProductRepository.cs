using System.Collections.Generic;

using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Application.Ports.Outbound
{
    public interface IProductRepository
    {
        IReadOnlyList<Product> GetAll();
        void SaveAll(IEnumerable<Product> products);
    }
}
