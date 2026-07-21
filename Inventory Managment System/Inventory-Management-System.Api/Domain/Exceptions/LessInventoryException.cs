using System;

namespace Inventory_Management_System.Api.Domain.Exceptions
{
    public class LessInventoryException : Exception
    {
        public LessInventoryException(int quantity)
            : base($"Low stock—there are only {quantity} units of this product left") { }
    }
}
