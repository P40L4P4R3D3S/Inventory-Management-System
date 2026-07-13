using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Domain.Exceptions
{
    public class DuplicateSkuException : Exception
    {
        public DuplicateSkuException(string sku)
            : base($"A product with SKU '{sku}' already exists.")
        {
        }
    }
}
