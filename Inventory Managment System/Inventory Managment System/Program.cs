using Inventory_Managment_System.Application.Interfaces;
using Inventory_Managment_System.Application.Services;
using Inventory_Managment_System.UI;
using Inventory_Managment_System.UI.Interfaces;
using System;

class Program
{
    public static void Main(string[] args)
    {
        IInventoryService inventoryService = new InventoryService();
        IConsoleInput consoleInput = new ConsoleInput();

        ConsoleMenu ui = new ConsoleMenu(inventoryService, consoleInput);

        ui.Run();

    }

}