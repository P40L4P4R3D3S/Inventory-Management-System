using System;
using System.Collections.Generic;
using Inventory_Managment_System.UI.Commands;
using Inventory_Managment_System.UI.Models;

namespace Inventory_Managment_System.UI.Menus
{
    public class ConsoleMenu
    {
        private readonly IReadOnlyDictionary<string, MenuOption> _mainOptions;

        public ConsoleMenu(
            IConsoleCommand addProductCommand,
            IConsoleCommand showProductsCommand,
            IConsoleCommand searchProductsMenuCommand,
            IConsoleCommand updateProductCommand,
            IConsoleCommand receiveProductCommand,
            IConsoleCommand shipProductCommand,
            IConsoleCommand productDetailCommand
        )
        {
            _mainOptions = new Dictionary<string, MenuOption>
            {
                { "1", new MenuOption("Add Product", addProductCommand) },
                { "2", new MenuOption("View Products", showProductsCommand) },
                { "3", new MenuOption("Search Products", searchProductsMenuCommand) },
                { "4", new MenuOption("Update Product", updateProductCommand) },
                { "5", new MenuOption("Receive Products", receiveProductCommand) },
                { "6", new MenuOption("Ship Products", shipProductCommand) },
                { "7", new MenuOption("View Product Detail", productDetailCommand) },
            };
        }

        public void Run()
        {
            MenuRunner.Run(
                title: "Inventory Management System",
                options: _mainOptions,
                exitOption: "8",
                exitDescription: "Exit"
            );
        }
    }
}
