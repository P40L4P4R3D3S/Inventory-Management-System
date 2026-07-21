using System;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Managment_System.UI.Input;

namespace Inventory_Managment_System.UI.Commands
{
    public class AddProductCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;

        public AddProductCommand(IInventoryService inventoryService, IConsoleInput consoleInput)
        {
            _inventoryService =
                inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));

            _consoleInput = consoleInput ?? throw new ArgumentNullException(nameof(consoleInput));
        }

        public void Execute()
        {
            string name = _consoleInput.ReadRequiredString("Name: ");

            string description = _consoleInput.ReadOptionalString("Description: ");

            string sku = _consoleInput.ReadRequiredString("SKU: ");

            decimal price = _consoleInput.ReadDecimal("Price: ");

            Product product = new(name, description, price, sku);

            _inventoryService.AddProduct(product);

            Console.WriteLine("Product added successfully.");
        }
    }
}
