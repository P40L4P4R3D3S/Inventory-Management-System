using System;

namespace Inventory_Management_System.Api.Domain.Exceptions
{
    public class DuplicateSkuException : Exception
    {
        public DuplicateSkuException(string sku)
            : base($"A product with SKU '{sku}' already exists.")
        {
        }
    }
}
