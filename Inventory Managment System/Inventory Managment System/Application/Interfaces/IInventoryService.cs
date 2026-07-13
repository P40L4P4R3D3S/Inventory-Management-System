using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Application.Interfaces
{
    public interface IInventoryService
    {
        void AddProduct(Product product);
        IReadOnlyList<Product> GetAllProducts();
    }
}
