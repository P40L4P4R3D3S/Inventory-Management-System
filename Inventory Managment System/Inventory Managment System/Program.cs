using Inventory_Managment_System.Application.Interfaces;
using Inventory_Managment_System.Application.Services;
using Inventory_Managment_System.UI;
using System;

class Program
{
    public static void Main(string[] args)
    {
        IInventoryService inventoryService = new InventoryService();

        ConsoleMenu ui = new ConsoleMenu(inventoryService);

        ui.Run();

    }

}