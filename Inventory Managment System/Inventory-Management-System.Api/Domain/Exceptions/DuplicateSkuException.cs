using System;

namespace Inventory_Management_System.Api.Domain.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string dataname, string data)
            : base($"A product with {dataname}: '{data}' already exists.") { }
    }
}
