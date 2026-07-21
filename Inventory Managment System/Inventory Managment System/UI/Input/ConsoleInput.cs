using System;
using System.Globalization;

namespace Inventory_Managment_System.UI.Input
{
    public class ConsoleInput : IConsoleInput
    {
        public string ReadRequiredString(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("This field cannot be empty.");
            }
        }

        public string ReadOptionalString(string message)
        {
            Console.Write(message);

            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(input, out decimal result))
                {
                    return result;
                }

                Console.WriteLine("The value must be a valid decimal number.");
            }
        }

        public int ReadInteger(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                Console.WriteLine("The value must be a valid integer.");
            }
        }

        public DateTime? ReadDateTime(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                if (
                    DateTime.TryParseExact(
                        input,
                        "dd-MM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime result
                    )
                )
                {
                    return result.Date;
                }

                Console.WriteLine("Invalid date. Please use the format DD-MM-YYYY.");
            }
        }
    }
}
