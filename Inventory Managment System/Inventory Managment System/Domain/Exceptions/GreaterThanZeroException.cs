using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Domain.Exceptions
{
    internal class GreaterThanZeroException : Exception
    {
        public GreaterThanZeroException(): base("The quantity must be greater than Zero")
        { }
    }
}
