using System;

namespace Inventory_Managment_System.UI.Input
{
    public interface IConsoleInput
    {
        string ReadRequiredString(string message);
        string ReadOptionalString(string message);
        decimal ReadDecimal(string message);
        int ReadInteger(string message);
        DateTime? ReadDateTime(string message);
    }
}
