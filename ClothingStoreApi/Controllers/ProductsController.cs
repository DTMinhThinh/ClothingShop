using AutoMapper;
using ClothingStore.Application.Commands.ProductCommand;
using ClothingStore.Application.DTOs.Product;
using ClothingStore.Application.Queries;
using ClothingStore.Application.Queries.ProductQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public ProductsController(IMediator mediator, IMapper mapper)
		{
			_mediator = mediator;
			_mapper = mapper;
		}

		// GET: api/products
		[HttpGet("")]
		public async Task<IActionResult> GetAllProducts()
		{
			try
			{
				var products = await _mediator.Send(new GetAllActiveProductsQuery());
				return Ok(products);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		// GET: api/products/{productId}
		[HttpGet("{productId}")]
		public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
		{
			try
			{
				var product = await _mediator.Send(new GetProductByIdQuery(productId));
				return Ok(product);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { Error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpPut("{productId}")]
		public async Task<IActionResult> UpdateProduct(
		[FromRoute] Guid productId,
		[FromBody] UpdateProductDTO dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _mediator.Send(new UpdateProductCommand(productId, dto));
				return result ? Ok() : NotFound();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { Error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}


		[HttpPost]
		public async Task<IActionResult> AddProduct([FromBody] CreateProductDTO createDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var product = await _mediator.Send(new AddProductCommand(
					createDto.CategoryID,
					createDto.ProductName,
					createDto.Description,
					createDto.Price,
					createDto.IsActive,
					createDto.ImageUrl
				));

				return Ok(new { Product = product });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Error = ex.Message });
			}
		}


		// DELETE: api/products/{productId}
		[HttpDelete("{productId}")]
		public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
		{
			try
			{
				var result = await _mediator.Send(new DeleteProductCommand(productId));
				return result ? NoContent() : NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}
	}
}
