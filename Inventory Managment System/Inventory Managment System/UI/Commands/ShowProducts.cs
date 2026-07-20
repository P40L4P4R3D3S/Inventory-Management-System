using System;
using System.Collections.Generic;

using Inventory_Managment_System.Application.Ports.Inbound;
using Inventory_Managment_System.Domain.Models;
using Inventory_Managment_System.UI.Enums;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    public class ShowProductsCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly ProductConsolePresenter _productPresenter;

        public ShowProductsCommand(
            IInventoryService inventoryService,
            ProductConsolePresenter productPresenter)
        {
            _inventoryService = inventoryService
                ?? throw new ArgumentNullException(
                    nameof(inventoryService));

            _productPresenter = productPresenter
                ?? throw new ArgumentNullException(
                    nameof(productPresenter));
        }

        public void Execute()
        {
            IReadOnlyList<Product> products =
                _inventoryService.GetAllProducts();

            _productPresenter.ShowProductList(
                products,
                ProductDisplayMode.Summary);
        }
    }
}