using System;
using System.Collections.Generic;
using System.Linq;

using Inventory_Managment_System.Application.Ports.Inbound;
using Inventory_Managment_System.Domain.Entities;
using Inventory_Managment_System.Domain.Models;
using Inventory_Managment_System.UI.Enums;
using Inventory_Managment_System.UI.Input;
using Inventory_Managment_System.UI.Models;

namespace Inventory_Managment_System.UI.Menus
{
    public class ConsoleMenu
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConsoleInput _consoleInput;

        public ConsoleMenu(
            IInventoryService inventoryService,
            IConsoleInput consoleInput)
        {
            _inventoryService = inventoryService
                ?? throw new ArgumentNullException(
                    nameof(inventoryService));

            _consoleInput = consoleInput
                ?? throw new ArgumentNullException(
                    nameof(consoleInput));
        }

        public void Run()
        {
            Dictionary<string, MenuOption> mainOptions = new()
            {
                {
                    "1",
                    new MenuOption(
                        "Add Product",
                        AddProduct)
                },
                {
                    "2",
                    new MenuOption(
                        "View Products",
                        ShowProducts)
                },
                {
                    "3",
                    new MenuOption(
                        "Search Products",
                        RunSearchMenu)
                },
                {
                    "4",
                    new MenuOption(
                        "Update Products",
                        UpdateProduct)
                },
                {
                    "5",
                    new MenuOption(
                        "Receive Products",
                        ReceiveProduct)
                },
                {
                    "6",
                    new MenuOption(
                        "Ship Products",
                        ShipProduct)
                },
                {
                    "7",
                    new MenuOption(
                        "View Product Detail",
                        ProductDetail)
                }
            };

            RunMenu(
                title: "Inventory Management System",
                options: mainOptions,
                exitOption: "8",
                exitDescription: "Exit");
        }

        private void ProductDetail()
        {
            int id =
                _consoleInput.ReadInteger("Id: ");
            Product? p = _inventoryService.GetProductById(id);
            ShowProduct(p);
        }

        private static void RunMenu(
            string title,
            Dictionary<string, MenuOption> options,
            string exitOption,
            string exitDescription)
        {
            bool running = true;

            while (running)
            {
                ShowMenu(
                    title,
                    options,
                    exitOption,
                    exitDescription);

                string selectedOption =
                    Console.ReadLine()?.Trim() ?? string.Empty;

                if (selectedOption == exitOption)
                {
                    running = false;
                    continue;
                }

                if (!options.TryGetValue(
                        selectedOption,
                        out MenuOption? menuOption))
                {
                    Console.WriteLine("Invalid option.");
                    continue;
                }

                try
                {
                    menuOption.Execute();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(
                        $"Error: {exception.Message}");
                }
            }
        }

        private static void ShowMenu(
            string title,
            Dictionary<string, MenuOption> options,
            string exitOption,
            string exitDescription)
        {
            Console.WriteLine();
            Console.WriteLine(title);

            foreach (KeyValuePair<string, MenuOption> option in options)
            {
                Console.WriteLine(
                    $"{option.Key}. {option.Value.Description}");
            }

            Console.WriteLine(
                $"{exitOption}. {exitDescription}");

            Console.Write("Choose an option: ");
        }

        private void AddProduct()
        {
            string name =
                _consoleInput.ReadRequiredString("Name: ");

            string description =
                _consoleInput.ReadOptionalString("Description: ");

            string sku =
                _consoleInput.ReadRequiredString("SKU: ");

            decimal price =
                _consoleInput.ReadDecimal("Price: ");

            Product product = new(
                name,
                description,
                price,
                sku);

            _inventoryService.AddProduct(product);

            Console.WriteLine(
                "Product added successfully.");
        }


        private void UpdateProduct()
        {
            ShowProducts();

            int id =
                _consoleInput.ReadInteger("Select a product by Id: ");
            decimal price =
                _consoleInput.ReadDecimal("Price: ");

            _inventoryService.UpdateProduct(id, price);

            Console.WriteLine(
                "Product updated successfully.");
        }

        private void ShowProducts()
        {
            IReadOnlyList<Product> products =
                _inventoryService.GetAllProducts();

            ShowProductList(products, ProductDisplayMode.Summary);
        }

        private void RunSearchMenu()
        {
            Dictionary<string, MenuOption> searchOptions = new()
            {
                {
                    "1",
                    new MenuOption(
                        "Search By Name",
                        SearchByName)
                },
                {
                    "2",
                    new MenuOption(
                        "Search By SKU",
                        SearchBySku)
                }
            };

            RunMenu(
                title: "Search Options",
                options: searchOptions,
                exitOption: "3",
                exitDescription: "Back Home");
        }

        private void SearchByName()
        {
            string name =
                _consoleInput.ReadRequiredString(
                    "Enter the product name: ");

            IReadOnlyList<Product> products =
                _inventoryService.SearchProductsByName(name);

            ShowProductList(products);
        }

        private void SearchBySku()
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
            ShowProduct(product, ProductDisplayMode.Summary);
        }


        private void ShipProduct()
        {
            string sku =
                _consoleInput.ReadRequiredString(
                    "Enter the product SKU: ");

            Product product =
                _inventoryService.GetProductBySku(sku);

            ShowLots(product.Lots);

            string lotNumber =
                _consoleInput.ReadRequiredString(
                    "Enter the lot number: ");

            int quantity =
                _consoleInput.ReadInteger(
                    "Quantity: ");

            InventoryLot lot =
                _inventoryService.ShipProduct(
                    sku,
                    lotNumber,
                    quantity);

            Console.WriteLine(
                "Product shipped successfully.");

            ShowLot(lot);
        }

        private void ReceiveProduct()
        {
            string sku =
                _consoleInput.ReadRequiredString(
                    "Enter the product SKU: ");

            int quantity = _consoleInput.ReadInteger("Quantity: ");

            string lotNumber =
                _consoleInput.ReadRequiredString("Lot number: ");

            DateTime receivedDate = DateTime.Now;
            DateTime? expirationDate = _consoleInput.ReadDateTime("Set Date Expiration (DD-MM-YYYY): ");
            string supplier =
                _consoleInput.ReadRequiredString("Supplier");

            InventoryLot lot = _inventoryService.ReceiveProduct(sku, lotNumber, quantity, receivedDate, expirationDate, supplier);
            ShowLot(lot);
            Console.WriteLine("Lot Added Succesfully");
        }

        private static void ShowProductList(
            IReadOnlyList<Product> products, ProductDisplayMode displayMode = ProductDisplayMode.Detailed)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine();
            string header = displayMode switch
            {
                ProductDisplayMode.Summary =>
                    "ID | Name | SKU ",

                ProductDisplayMode.Detailed =>
                    "ID | Name | Description | SKU | Price ",

                _ => throw new ArgumentOutOfRangeException(
                    nameof(displayMode),
                    displayMode,
                    "Invalid product display mode.")
            };

            Console.WriteLine(header);
            foreach (Product product in products)
            {
                ShowProduct(product, displayMode);
            }
        }

        private static void ShowProduct(Product product, ProductDisplayMode displayMode = ProductDisplayMode.Detailed)
        {
            string productInformation = displayMode switch
            {
                ProductDisplayMode.Summary =>
                    $"{product.Id} | " +
                    $"{product.Name} | " +
                    $"{product.SKU}",

                ProductDisplayMode.Detailed =>
                    $"{product.Id} | " +
                    $"{product.Name} | " +
                    $"{product.Description} | " +
                    $"{product.SKU} | " +
                    $"{product.Price:F2} | ",

                _ => throw new ArgumentOutOfRangeException(
                    nameof(displayMode),
                    displayMode,
                    "Invalid product display mode.")
            };

            Console.WriteLine(productInformation);
        }

        private static void ShowLots(IReadOnlyList<InventoryLot> lots)
        {
            IReadOnlyList<InventoryLot> availableLots =
                lots
                    .Where(lot => lot.QuantityOnHand > 0)
                    .ToList();

            if (availableLots.Count == 0)
            {
                Console.WriteLine(
                    "There are no available lots.");

                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Lot | Received | Expiration | Available");

            foreach (InventoryLot lot in availableLots)
            {
                ShowLot(lot);
            }
        }

        private static void ShowLot(InventoryLot lot)
        {
            string expirationDate =
                lot.ExpirationDate?.ToString("dd-MM-yyyy")
                ?? "N/A";

            Console.WriteLine(
                $"{lot.LotNumber} | " +
                $"{lot.ReceivedDate:dd-MM-yyyy} | " +
                $"{expirationDate} | " +
                $"{lot.QuantityOnHand}");
        }
    }
}