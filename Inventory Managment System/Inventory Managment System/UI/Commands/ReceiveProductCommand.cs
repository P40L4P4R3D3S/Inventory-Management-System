using System;

using Inventory_Managment_System.Application.Ports.Inbound;
using Inventory_Managment_System.Domain.Entities;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    public class ReceiveProductCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;
        private readonly LotConsolePresenter _lotPresenter;

        public ReceiveProductCommand(
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
            string sku = _consoleInput.ReadRequiredString("Enter the product SKU: ");
            int quantity = _consoleInput.ReadInteger("Quantity: ");
            string lotNumber = _consoleInput.ReadRequiredString("Lot number: ");
            DateTime receivedDate = DateTime.Now;
            DateTime? expirationDate = _consoleInput.ReadDateTime("Expiration date (DD-MM-YYYY): ");
            string supplier = _consoleInput.ReadRequiredString("Supplier: ");

            InventoryLot lot = _inventoryService.ReceiveProduct(
                    sku,
                    lotNumber,
                    quantity,
                    receivedDate,
                    expirationDate,
                    supplier);

            Console.WriteLine("Lot added successfully.");

            _lotPresenter.ShowLot(lot);
        }
    }
}