using System.Collections.Generic;

using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;

using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Presenters;

namespace Inventory_Managment_System.UI.Commands
{
    public class SearchByNameCommand : IConsoleCommand
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;
        private readonly ProductConsolePresenter _productPresenter;

        public SearchByNameCommand(
            IInventoryService inventoryService,
            IConsoleInput consoleInput,
            ProductConsolePresenter productPresenter)
        {
            _inventoryService = inventoryService;
            _consoleInput = consoleInput;
            _productPresenter = productPresenter;
        }

        public void Execute()
        {
            string name = _consoleInput.ReadRequiredString("Enter the product name: ");

            IReadOnlyList<Product> products = _inventoryService.SearchProductsByName(name);

            _productPresenter.ShowProductList(products);
        }
    }
}
