using System;
using System.IO;

using Inventory_Managment_System.Application.Ports.Inbound;
using Inventory_Managment_System.Application.Ports.Outbound;
using Inventory_Managment_System.Application.Services;
using Inventory_Managment_System.Infrastructure.Persistence;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Menus;

class Program
{
    public static void Main(string[] args)
    {
        string dataDirectory =
            Path.Combine(
                AppContext.BaseDirectory,
                "Data");

        string productsPath =
            Path.Combine(
                dataDirectory,
                "Products.json");

        string transactionsPath =
            Path.Combine(
                dataDirectory,
                "Transactions.json");

        Console.WriteLine(
            "Reading products...");

        IProductRepository productRepository =
            new ProductRepository(
                productsPath);

        Console.WriteLine(
            "Reading transactions...");

        ITransactionRepository transactionRepository =
            new TransactionRepository(
                transactionsPath);

        IInventoryService inventoryService = new InventoryService(productRepository, transactionRepository);

        IConsoleInput consoleInput = new ConsoleInput();

        ConsoleMenu ui = new ConsoleMenu(inventoryService, consoleInput);

        Console.WriteLine(
            "Application ready.");

        ui.Run();
    }
}