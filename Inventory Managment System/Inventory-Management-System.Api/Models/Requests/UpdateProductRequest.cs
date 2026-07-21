using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.Api.Models.Requests
{
    public class UpdateProductRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }
    }
}
