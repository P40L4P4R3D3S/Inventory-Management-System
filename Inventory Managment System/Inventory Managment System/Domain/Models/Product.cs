using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Managment_System.Domain.Models
{
    public class Product
    {
        private int _id;
        private string _name;
        private string _description;
        private decimal _price;
        private string _sku;
        private int _quantityOnHand;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public decimal Price { get => _price; set => _price = value; }
        public string SKU { get => _sku; set => _sku = value; }
        public int QuantityOnHand { get => _quantityOnHand; set => _quantityOnHand = value; }

        public Product(string name, string description, decimal price, string sku, int quantityOnHand)

        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Product name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException(
                    "SKU cannot be empty.");
            }

            if (price < 0)
            {
                throw new ArgumentException(
                    "Price cannot be negative.");
            }

            if (quantityOnHand < 0)
            {
                throw new ArgumentException(
                    "Quantity cannot be negative.");
            }

            Name = name;
            Description = description;
            SKU = sku;
            Price = price;
            QuantityOnHand = quantityOnHand;
        }

        public void AssignId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Product ID must be greater than zero.");
            }

            Id = id;
        }


    }
}
