using System;

using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;

using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    public class ShipProductCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;
        private readonly LotConsolePresenter _lotPresenter;

        public ShipProductCommand(
            IInventoryService inventoryService,
            IConsoleInput consoleInput,
            LotConsolePresenter lotPresenter)
        {
            _inventoryService = inventoryService;
            _consoleInput = consoleInput;
            _lotPresenter = lotPresenter;
        }

        public void Execute()
        {
            string sku =
                _consoleInput.ReadRequiredString(
                    "Enter the product SKU: ");

            Product? product =
                _inventoryService.GetProductBySku(sku);

            if (product is null)
            {
                Console.WriteLine("No product was found with that SKU.");

                return;
            }

            bool hasAvailableLots = _lotPresenter.ShowAvailableLots(product.Lots);

            if (!hasAvailableLots)
            {
                return;
            }

            string lotNumber = _consoleInput.ReadRequiredString("Enter the lot number: ");

            int quantity = _consoleInput.ReadInteger("Quantity to ship: ");

            InventoryLot updatedLot = _inventoryService.ShipProduct(sku, lotNumber, quantity);

            Console.WriteLine("Product shipped successfully.");

            _lotPresenter.ShowLot(updatedLot);
        }
    }
}
