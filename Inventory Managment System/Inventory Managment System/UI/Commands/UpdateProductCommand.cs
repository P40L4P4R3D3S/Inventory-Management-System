using System;
using System.Collections.Generic;

using Inventory_Managment_System.Application.Ports.Inbound;
using Inventory_Managment_System.Domain.Models;
using Inventory_Managment_System.UI.Enums;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    internal class UpdateProductCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;
        private readonly ProductConsolePresenter _productPresenter;
        public UpdateProductCommand(
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
            IReadOnlyList<Product> products =
                _inventoryService.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine(
                    "There are no products to update.");

                return;
            }

            _productPresenter.ShowProductList(
                products,
                ProductDisplayMode.Summary);

            int id = _consoleInput.ReadInteger(
                "Select a product by Id: ");

            decimal price = _consoleInput.ReadDecimal(
                "New price: ");

            _inventoryService.UpdateProduct(
                id,
                price);

            Console.WriteLine(
                "Product updated successfully.");
        }
    }
}