using Inventory_Managment_System.Application.Repository;
using Inventory_Managment_System.Application.Repository.Interfaces;
using Inventory_Managment_System.Application.Services;
using Inventory_Managment_System.Application.Services.Interfaces;
using Inventory_Managment_System.Data.Repositories;
using Inventory_Managment_System.UI;
using Inventory_Managment_System.UI.Interfaces;
using System;
using System.IO;

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