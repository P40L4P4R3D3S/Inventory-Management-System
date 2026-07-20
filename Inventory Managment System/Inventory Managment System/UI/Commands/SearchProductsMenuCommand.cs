using System;
using System.Collections.Generic;

using Inventory_Managment_System.UI.Menus;
using Inventory_Managment_System.UI.Models;

namespace Inventory_Managment_System.UI.Commands
{
    public class SearchProductsMenuCommand : IConsoleCommand
    {
        private readonly Dictionary<string, MenuOption> _searchOptions;

        public SearchProductsMenuCommand(
            IConsoleCommand searchByNameCommand,
            IConsoleCommand searchBySkuCommand)
        {
            ArgumentNullException.ThrowIfNull(
                searchByNameCommand);

            ArgumentNullException.ThrowIfNull(
                searchBySkuCommand);

            _searchOptions = new Dictionary<string, MenuOption>
            {
                {
                    "1",
                    new MenuOption("Search by name", searchByNameCommand)
                },
                {
                    "2",
                    new MenuOption("Search by SKU", searchBySkuCommand)
                }
            };
        }

        public void Execute()
        {
            MenuRunner.Run(
                title: "Search Options",
                options: _searchOptions,
                exitOption: "3",
                exitDescription: "Back");
        }
    }
}