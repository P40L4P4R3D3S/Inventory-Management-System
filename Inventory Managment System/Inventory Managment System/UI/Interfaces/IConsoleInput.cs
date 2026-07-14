using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.UI.Interfaces
{
    public interface IConsoleInput
    {
        string ReadRequiredString(string message);
        string ReadOptionalString(string message);
        decimal ReadDecimal(string message);
        int ReadInteger(string message);
    }
}
