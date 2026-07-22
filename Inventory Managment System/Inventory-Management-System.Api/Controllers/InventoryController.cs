using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Records;
using Inventory_Management_System.Api.Infrastructure.Auth;
using Inventory_Management_System.Api.Models.Requests;
using Inventory_Management_System.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Api.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Authorize(Roles = AppRoles.Management)]
        [HttpPost("receive")]
        [ProducesResponseType(typeof(ReceiveProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ReceiveProductResponse> ReceiveProduct(ReceiveProductRequest request)
        {
            InventoryLot lot = _inventoryService.ReceiveProduct(
                request.Sku,
                request.LotNumber,
                request.Quantity,
                request.ReceivedDate,
                request.ExpirationDate,
                request.Supplier
            );

            Product product = _inventoryService.GetProductBySku(request.Sku);

            ReceiveProductResponse response = ReceiveProductResponse.FromDomain(product, lot);

            return CreatedAtAction(
                nameof(GetLot),
                new { sku = product.SKU, lotNumber = lot.LotNumber },
                response
            );
        }

        [Authorize]
        [HttpGet("{sku}/lots/{lotNumber}")]
        [ProducesResponseType(typeof(InventoryLotResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<InventoryLotResponse> GetLot(string sku, string lotNumber)
        {
            InventoryLot lot = _inventoryService.GetLot(sku, lotNumber);

            return Ok(InventoryLotResponse.FromDomain(lot));
        }

        [Authorize(Roles = AppRoles.Management)]
        [HttpPost("ship")]
        [ProducesResponseType(typeof(ShipProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ShipProductResponse> ShipProduct(ShipProductRequest request)
        {
            ShipProductResult result = _inventoryService.ShipProduct(
                request.ProductId,
                request.Quantity,
                request.LotId,
                request.Notes
            );

            ShipProductResponse response = ShipProductResponse.FromApplication(result);

            return Ok(response);
        }
    }
}
