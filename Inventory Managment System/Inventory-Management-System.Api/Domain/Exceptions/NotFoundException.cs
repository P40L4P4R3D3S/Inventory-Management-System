using System;

namespace Inventory_Management_System.Api.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base($"{message}")
        {
        }
    }
}
