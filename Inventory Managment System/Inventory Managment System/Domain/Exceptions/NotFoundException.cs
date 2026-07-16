using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base($"{message}")
        {
        }
    }
}
