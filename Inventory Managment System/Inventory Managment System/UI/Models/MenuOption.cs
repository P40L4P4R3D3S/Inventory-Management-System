using System;

namespace Inventory_Managment_System.UI.Models
{
    public class MenuOption
    {
        public string Description { get; }
        public Action Execute { get; }

        public MenuOption(string description, Action execute)
        {
            Description = description
                ?? throw new ArgumentNullException(nameof(description));

            Execute = execute
                ?? throw new ArgumentNullException(nameof(execute));
        }
    }
}
