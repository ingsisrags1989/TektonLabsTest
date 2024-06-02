using Application.Commands;
using Application.Dto;
using Application.Queries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Products.Api.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : Controller
    {

        private readonly IMediator _mediator;
        public ProductController(
            IMediator mediator
            )
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Finds a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to find.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <response code="200">Returns the product with the specified ID.</response>
        /// <response code="404">If the product is not found.</response>
        /// <response code="500">If exists internal error. </response>
        /// 
        [HttpGet("v1/get-product-by-id/{id}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [SwaggerOperation(
            Summary = "Get product by ID",
            Description = "Retrieves a product byID .",
            OperationId = "GetProductById"
        )]
        public async Task<ProductDto> GetPropertiesById([FromRoute] int Id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery() { Id = Id });
            return product;
        }


        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="command">For execute create product command.</param>
        /// <returns>The created product.</returns>
        /// <response code="200">Returns the created product.</response>
        /// <response code="400">If the product data is invalid.</response>
        /// <response code="500">If exist internal error.</response>
        [HttpPost("v1/create-product")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
            Summary = "Create a new product",
            Description = "Creates a new product",
            OperationId = "CreateProduct",
            Tags = new[] { "Product" }
        )]
        public async Task<ProductDto> CreateProduct([FromForm] CreateProductCommand command)
        {
            var product = await _mediator.Send(command);

            return product;

        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="command">The command is the body for execute command.</param>
        /// <returns>The updated product.</returns>
        /// <response code="200">Returns the updated product.</response>
        /// <response code="400">If the product data is invalid.</response>
        /// <response code="404">If the product is not found.</response>
        /// <response code="500">If exist internal error.</response>
        [HttpPut("v1/update-product")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [SwaggerOperation(
            Summary = "Update an existing product",
            Description = "Updates an existing product in the database.",
            OperationId = "UpdateProduct",
            Tags = new[] { "Product" }
        )]
        public async Task<ProductDto> UpdateProduct([FromForm] UpdateProductCommand command)
        {
            var product = await _mediator.Send(command);

            return product;

        }


    }
}
