using Application.Commands;
using Application.Dto;
using Application.Query;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Products.Api.Controllers
{
    public class ProductController : Controller
    {
        [Route("product")]
        [ApiController]
        public class PropertyController : ControllerBase
        {
            private readonly IMediator _mediator;
            public PropertyController(
                IMediator mediator
                )
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Method find product by Id
            /// </summary>
            /// <param name="Id"></param>
            /// <returns name="product"> </returns>

            [HttpGet("v1/get-product-by-id/{Id}")]
            [ProducesResponseType(typeof(Response<ProductDto>), (int)HttpStatusCode.OK)]
            public async Task<ProductDto> GetPropertiesById([FromRoute] Guid Id)
            {
                var product = await _mediator.Send(new GetProductByIdQuery() { Id = Id });
                return product;
            }


            /// <summary>
            /// Method for create product 
            /// </summary>
            /// <param name="command"></param>
            /// <returns name="product"></returns>

            [HttpPost("v1/create-product")]
            [ProducesResponseType(typeof(Response<ProductDto>), (int)HttpStatusCode.OK)]
            public async Task<ProductDto> CreateProduct([FromForm] CreateProductCommand command)
            {
                var product = await _mediator.Send(command);

                return product;

            }

            /// <summary>
            /// Method for update product
            /// </summary>
            /// <param name="command"></param>
            /// <returns name="product"></returns>


            [HttpPut("v1/update-product")]
            [ProducesResponseType(typeof(Response<ProductDto>), (int)HttpStatusCode.OK)]
            public async Task<ProductDto> UpdateProduct([FromForm] UpdateProductCommand command)
            {
                var product = await _mediator.Send(command);

                return product;

            }


        }
    }
}
