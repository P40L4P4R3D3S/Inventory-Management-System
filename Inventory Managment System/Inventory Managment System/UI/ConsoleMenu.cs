using Inventory_Managment_System.Application.Interfaces;
using Inventory_Managment_System.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.UI
{

    public class ConsoleMenu
    {
        private readonly IInventoryService _inventoryService;

        public ConsoleMenu(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                ShowOptions();

                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddProduct();
                        break;

                    case "2":
                        ShowProducts();
                        break;

                    case "3":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void ShowOptions()
        {
            Console.WriteLine();
            Console.WriteLine("Inventory Management System");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View Products");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");
        }

        private void AddProduct()
        {
            try
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Description: ");
                string description = Console.ReadLine() ?? string.Empty;

                Console.Write("SKU: ");
                string sku = Console.ReadLine();

                Console.Write("Price: ");
                decimal price = ReadDecimal();

                Console.Write("Quantity: ");
                int quantity = ReadInteger();

                Product product = new(
                    name,
                    description,
                    price,
                    sku,
                    quantity);

                _inventoryService.AddProduct(product);

                Console.WriteLine("Product added successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }

        private void ShowProducts()
        {
            IReadOnlyList<Product> products =
                _inventoryService.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products registered.");
                return;
            }

            foreach (Product product in products)
            {
                Console.WriteLine(
                    $"{product.Id} | " +
                    $"{product.Name} | " +
                    $"{product.Description}" +
                    $"{product.SKU} | " +
                    $"{product.Price:F2} | " +
                    $"{product.QuantityOnHand}");
            }
        }

        private static decimal ReadDecimal()
        {
            string input = Console.ReadLine() ?? string.Empty;

            if (!decimal.TryParse(input, out decimal result))
            {
                throw new ArgumentException(
                    "The price must be a valid number.");
            }

            return result;
        }

        private static int ReadInteger()
        {
            string input = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(input, out int result))
            {
                throw new ArgumentException(
                    "The quantity must be a valid integer.");
            }

            return result;
        }
    }
}
