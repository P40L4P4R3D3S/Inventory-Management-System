using Inventory_Managment_System.Application.Interfaces;
using Inventory_Managment_System.Domain.Models;
using Inventory_Managment_System.UI.Interfaces;
using Inventory_Managment_System.UI.Models;
using System;
using System.Collections.Generic;

namespace Inventory_Managment_System.UI
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
            Product? p = _inventoryService.SearchProductsById(id);
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

            int quantity =
                _consoleInput.ReadInteger("Quantity: ");

            Product product = new(
                name,
                description,
                price,
                sku,
                quantity);

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

            int quantity =
                _consoleInput.ReadInteger("Quantity: ");

            _inventoryService.UpdateProduct(id, price, quantity);

            Console.WriteLine(
                "Product updated successfully.");
        }

        private void ShowProducts()
        {
            IReadOnlyList<Product> products =
                _inventoryService.GetAllProducts();

            ShowProductList(products);
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
                _inventoryService.SearchProductsBySKU(sku);

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

            int quantity =
                _consoleInput.ReadInteger("Quantity: ");
            Product? p = _inventoryService.ShipProduct(sku, quantity);
            ShowProduct(p);
        }

        private void ReceiveProduct()
        {
            string sku =
                _consoleInput.ReadRequiredString(
                    "Enter the product SKU: ");

            int quantity =
                _consoleInput.ReadInteger("Quantity: "); ;
            Product? p = _inventoryService.ReceiveProduct(sku, quantity);
            ShowProduct(p);
        }

        private static void ShowProductList(
            IReadOnlyList<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "ID | Name | Description | SKU | Price | Quantity");

            foreach (Product product in products)
            {
                ShowProduct(product);
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
                    $"{product.Price:F2} | " +
                    $"{product.QuantityOnHand}",

                _ => throw new ArgumentOutOfRangeException(
                    nameof(displayMode),
                    displayMode,
                    "Invalid product display mode.")
            };

            Console.WriteLine(productInformation);
        }
    }
}