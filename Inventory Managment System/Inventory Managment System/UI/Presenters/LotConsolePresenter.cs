using System;
using System.Collections.Generic;
using System.Linq;

using Inventory_Managment_System.Domain.Entities;

namespace Inventory_Managment_System.UI.Presenters
{
    public class LotConsolePresenter
    {
        public void ShowLot(InventoryLot lot)
        {
            ArgumentNullException.ThrowIfNull(lot);

            string expirationDate =
                lot.ExpirationDate?.ToString("dd-MM-yyyy")
                ?? "N/A";

            Console.WriteLine(
                $"{lot.LotNumber} | " +
                $"{lot.ReceivedDate:dd-MM-yyyy} | " +
                $"{expirationDate} | " +
                $"{lot.QuantityOnHand}");
        }

        public bool ShowAvailableLots(
            IReadOnlyList<InventoryLot> lots)
        {
            ArgumentNullException.ThrowIfNull(lots);

            IReadOnlyList<InventoryLot> availableLots =
                lots
                    .Where(lot => lot.QuantityOnHand > 0)
                    .ToList();

            if (availableLots.Count == 0)
            {
                Console.WriteLine(
                    "There are no available lots.");

                return false;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Lot | Received | Expiration | Available");

            foreach (InventoryLot lot in availableLots)
            {
                ShowLot(lot);
            }

            return true;
        }
    }
}