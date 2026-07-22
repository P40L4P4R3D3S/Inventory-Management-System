using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.Api.Models.Requests
{
    public class ShipProductRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be greater than zero.")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lot ID must be greater than zero.")]
        public int? LotId { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot contain more than 500 characters.")]
        public string? Notes { get; set; }
    }
}
