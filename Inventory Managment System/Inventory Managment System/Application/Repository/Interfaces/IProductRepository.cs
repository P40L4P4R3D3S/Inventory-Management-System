using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Application.Repository.Interfaces
{
    public interface IProductRepository
    {
        IReadOnlyList<Product> GetAll();
        void SaveAll(IEnumerable<Product> products);
    }
}
