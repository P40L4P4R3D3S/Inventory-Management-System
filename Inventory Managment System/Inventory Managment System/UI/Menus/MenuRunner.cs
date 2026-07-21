using System;
using System.Collections.Generic;
using Inventory_Managment_System.UI.Models;

namespace Inventory_Managment_System.UI.Menus
{
    public static class MenuRunner
    {
        public static void Run(
            string title,
            IReadOnlyDictionary<string, MenuOption> options,
            string exitOption,
            string exitDescription
        )
        {
            while (true)
            {
                ShowMenu(title, options, exitOption, exitDescription);

                string selectedOption = Console.ReadLine()?.Trim() ?? string.Empty;

                if (selectedOption == exitOption)
                {
                    return;
                }

                if (!options.TryGetValue(selectedOption, out MenuOption? menuOption))
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
                    Console.WriteLine($"Error: {exception.Message}");
                }
            }
        }

        private static void ShowMenu(
            string title,
            IReadOnlyDictionary<string, MenuOption> options,
            string exitOption,
            string exitDescription
        )
        {
            Console.WriteLine();
            Console.WriteLine(title);

            foreach (KeyValuePair<string, MenuOption> option in options)
            {
                Console.WriteLine($"{option.Key}. " + $"{option.Value.Description}");
            }

            Console.WriteLine($"{exitOption}. {exitDescription}");

            Console.Write("Choose an option: ");
        }
    }
}
