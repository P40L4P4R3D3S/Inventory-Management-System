using System;
using System.Collections.Generic;

using Inventory_Managment_System.Domain.Models;
using Inventory_Managment_System.UI.Enums;

namespace Inventory_Managment_System.UI.Presenters
{
    public class ProductConsolePresenter
    {
        public void ShowProductList(
            IReadOnlyList<Product> products,
            ProductDisplayMode displayMode =
                ProductDisplayMode.Detailed)
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
                    "ID | Name | SKU",

                ProductDisplayMode.Detailed =>
                    "ID | Name | Description | SKU | Price",

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

        public void ShowProduct(
            Product product,
            ProductDisplayMode displayMode =
                ProductDisplayMode.Detailed)
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
                    $"{product.Price:F2}",

                _ => throw new ArgumentOutOfRangeException(
                    nameof(displayMode),
                    displayMode,
                    "Invalid product display mode.")
            };

            Console.WriteLine(productInformation);
        }
    }
}