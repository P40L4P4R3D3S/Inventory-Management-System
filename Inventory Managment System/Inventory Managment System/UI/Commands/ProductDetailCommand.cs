using System;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    public class ProductDetailCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;
        private readonly ProductConsolePresenter _productPresenter;

        public ProductDetailCommand(
            IInventoryService inventoryService,
            IConsoleInput consoleInput,
            ProductConsolePresenter productPresenter
        )
        {
            _inventoryService = inventoryService;
            _consoleInput = consoleInput;
            _productPresenter = productPresenter;
        }

        public void Execute()
        {
            int id = _consoleInput.ReadInteger("Enter the product ID: ");

            Product? product = _inventoryService.GetProductById(id);

            if (product is null)
            {
                Console.WriteLine("No product was found with that ID.");
                return;
            }

            _productPresenter.ShowProduct(product);
        }
    }
}
