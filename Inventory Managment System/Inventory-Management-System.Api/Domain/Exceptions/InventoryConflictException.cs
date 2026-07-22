using System;

namespace Inventory_Management_System.Api.Domain.Exceptions
{
    public class InventoryConflictException : Exception
    {
        public InventoryConflictException(string message)
            : base(message) { }
    }
}
