using System;
using System.Collections.Generic;
using System.Linq;

using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Exceptions;
using Inventory_Management_System.Api.Models.Requests;
using Inventory_Management_System.Api.Models.Responses;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public ProductsController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService
                ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
        public ActionResult<IReadOnlyList<ProductResponse>> GetAll()
        {
            IReadOnlyList<ProductResponse> response = _inventoryService
                    .GetAllProducts()
                    .Select(ProductResponse.FromDomain)
                    .ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> GetById(int id)
        {
            try
            {
                Product product = _inventoryService.GetProductById(id);
                ProductResponse response = ProductResponse.FromDomain(product);
                return Ok(response);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                return BadRequest(new { message = exception.Message });
            }
            catch (NotFoundException exception)
            {
                return NotFound(new { message = exception.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ProductResponse> Create(CreateProductRequest request)
        {
            try
            {
                Product product = new(
                    request.Name,
                    request.Description,
                    request.Price,
                    request.SKU);

                _inventoryService.AddProduct(product);

                ProductResponse response = ProductResponse.FromDomain(product);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        id = product.Id
                    },
                    response);
            }
            catch (DuplicateSkuException exception)
            {
                return Conflict(
                    new
                    {
                        message = exception.Message
                    });
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    new
                    {
                        message = exception.Message
                    });
            }
        }
    }
}
