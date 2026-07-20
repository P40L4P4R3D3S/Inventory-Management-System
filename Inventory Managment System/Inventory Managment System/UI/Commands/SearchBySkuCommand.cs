using System;

using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;

using Inventory_Managment_System.UI.Enums;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    public class SearchBySkuCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;
        private readonly ProductConsolePresenter _productPresenter;

        public SearchBySkuCommand(
            IInventoryService inventoryService,
            IConsoleInput consoleInput,
            ProductConsolePresenter productPresenter)
        {
            _inventoryService = inventoryService
                ?? throw new ArgumentNullException(
                    nameof(inventoryService));

            _consoleInput = consoleInput
                ?? throw new ArgumentNullException(
                    nameof(consoleInput));

            _productPresenter = productPresenter
                ?? throw new ArgumentNullException(
                    nameof(productPresenter));
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
                Console.WriteLine(
                    "No product was found with that SKU.");

                return;
            }

            _productPresenter.ShowProduct(
                product,
                ProductDisplayMode.Summary);
        }
    }
}
