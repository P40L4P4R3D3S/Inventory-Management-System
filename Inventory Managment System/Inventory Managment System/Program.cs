using System;
using System.IO;

using Inventory_Managment_System.Application.Ports.Inbound;
using Inventory_Managment_System.Application.Ports.Outbound;
using Inventory_Managment_System.Application.Services;
using Inventory_Managment_System.Infrastructure.Persistence;
using Inventory_Managment_System.UI.Commands;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Menus;
using Inventory_Managment_System.UI.Presenters;

class Program
{
    public static void Main(string[] args)
    {
        string dataDirectory =
            Path.Combine(AppContext.BaseDirectory, "Data");

        string productsPath = Path.Combine(dataDirectory, "Products.json");

        string transactionsPath = Path.Combine(dataDirectory, "Transactions.json");

        Console.WriteLine("Reading products...");

        IProductRepository productRepository = new ProductRepository(productsPath);

        Console.WriteLine("Reading transactions...");

        ITransactionRepository transactionRepository = new TransactionRepository(transactionsPath);

        ProductConsolePresenter productPresenter = new ProductConsolePresenter();
        LotConsolePresenter lotPresenter = new LotConsolePresenter();

        IInventoryService inventoryService = new InventoryService(productRepository, transactionRepository);
        IConsoleInput consoleInput = new ConsoleInput();
        IConsoleCommand addProductCommand = new AddProductCommand(inventoryService, consoleInput);
        IConsoleCommand showProductsCommand = new ShowProductsCommand(inventoryService, productPresenter);
        IConsoleCommand receiveProductCommand = new ReceiveProductCommand(
            inventoryService, consoleInput, lotPresenter);
        IConsoleCommand searchByNameCommand = new SearchByNameCommand(
            inventoryService, consoleInput, productPresenter);
        IConsoleCommand searchBySkuCommand = new SearchBySkuCommand(
            inventoryService, consoleInput, productPresenter);
        IConsoleCommand searchProductsMenuCommand = new SearchProductsMenuCommand(
            searchByNameCommand, searchBySkuCommand);
        IConsoleCommand updateProductCommand = new UpdateProductCommand(
            inventoryService, consoleInput, productPresenter);
        IConsoleCommand productDetailCommand = new ProductDetailCommand(
            inventoryService, consoleInput, productPresenter);
        IConsoleCommand shipProductCommand = new ShipProductCommand(
            inventoryService, consoleInput, lotPresenter);

        ConsoleMenu ui = new ConsoleMenu(
            addProductCommand,
            showProductsCommand,
            searchProductsMenuCommand,
            updateProductCommand,
            receiveProductCommand,
            shipProductCommand,
            productDetailCommand);

        Console.WriteLine("Application ready.");

        ui.Run();
    }
}
