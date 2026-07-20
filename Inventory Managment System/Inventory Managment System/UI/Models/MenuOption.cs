using System;

using Inventory_Managment_System.UI.Commands;

namespace Inventory_Managment_System.UI.Models
{
    public class MenuOption
    {
        private readonly IConsoleCommand _command;

        public string Description { get; }

        public MenuOption(string description, IConsoleCommand command)
        {
            Description = description;
            _command = command;
        }

        public void Execute()
        {
            _command.Execute();
        }
    }
}