using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.Api.API.Models.Requests
{
    public class ReceiveProductRequest
    {
        [Required]
        public string Sku { get; set; } = string.Empty;

        [Required]
        public string LotNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string Supplier { get; set; } = string.Empty;
    }
}
