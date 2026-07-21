using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Models.Responses
{
    public class ProductResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string SKU { get; init; } = string.Empty;
        public int QuantityOnHand { get; init; }

        public static ProductResponse FromDomain(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                QuantityOnHand = product.QuantityOnHand,
            };
        }
    }
}
