using System;
using System.Collections.Generic;
using System.Linq;
using Inventory_Management_System.Api.API.Models.Requests;
using Inventory_Management_System.Api.API.Models.Responses;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Api.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public ProductsController(IInventoryService inventoryService)
        {
            _inventoryService =
                inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaginatedResponse<ProductResponse>> GetAll(
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            IReadOnlyList<Product> products = string.IsNullOrWhiteSpace(name)
                ? _inventoryService.GetAllProducts(pageNumber, pageSize)
                : _inventoryService.SearchProductsByName(name, pageNumber, pageSize);

            int totalItems = _inventoryService.GetProductsCount(name);

            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            PaginatedResponse<ProductResponse> response = new()
            {
                Items = products.Select(ProductResponse.FromDomain).ToList(),

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> GetById(int id)
        {
            Product product = _inventoryService.GetProductById(id);

            ProductResponse response = ProductResponse.FromDomain(product);

            return Ok(response);
        }

        [Authorize(Roles = AppRoles.InventoryOperators)]
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ProductResponse> Create(CreateProductRequest request)
        {
            Product product = new(request.Name, request.Description, request.Price, request.SKU);

            _inventoryService.AddProduct(product);

            ProductResponse response = ProductResponse.FromDomain(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
        }

        [Authorize(Roles = AppRoles.Management)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateProduct(int id, UpdateProductRequest request)
        {
            _inventoryService.UpdateProduct(id, request.Price, request.Name, request.Description);

            return NoContent();
        }

        [Authorize(Roles = AppRoles.Management)]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int id)
        {
            _inventoryService.DeleteProduct(id);

            return NoContent();
        }
    }
}
