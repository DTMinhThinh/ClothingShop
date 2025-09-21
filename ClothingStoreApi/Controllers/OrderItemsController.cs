using ClothingStore.Application.Commands.OrderItemCommand;
using ClothingStore.Application.Queries.OrderItemQueries;
using ClothingStore.Application.Queries.OrderQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderItemsController : ControllerBase
	{
		private readonly IMediator _mediator;


		public OrderItemsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("order/{orderId}")]
		public async Task<IActionResult> GetItemsByOrderId([FromRoute] Guid orderId)
		{
			try
			{
				var items = await _mediator.Send(new GetOrderItemsByOrderIdQuery(orderId));
				return Ok(items);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("product/{productId}")]
		public async Task<IActionResult> GetItemsByProductId([FromRoute] Guid productId)
		{
			try
			{
				var items = await _mediator.Send(new GetOrderItemsByProductIdQuery(productId));
				return Ok(items);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpPut("{orderId}/{productId}")]
		public async Task<IActionResult> UpdateOrderItem(
			[FromRoute] Guid orderId,
			[FromRoute] Guid productId,
			[FromBody] UpdateOrderItemCommand command)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				// Đảm bảo orderId và productId trong route khớp với command
				var result = await _mediator.Send(command with { OrderId = orderId, ProductId = productId });
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpDelete("{orderId}/{productId}")]
		public async Task<IActionResult> DeleteOrderItem(
			[FromRoute] Guid orderId,
			[FromRoute] Guid productId)
		{
			try
			{
				var result = await _mediator.Send(new DeleteOrderItemCommand(orderId, productId));
				return result ? NoContent() : NotFound();
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("{orderId}/{productId}")]
		public async Task<IActionResult> GetOrderItemById(
			[FromRoute] Guid orderId,
			[FromRoute] Guid productId)
		{
			try
			{
				var item = await _mediator.Send(new GetOrderItemByIdQuery(orderId, productId));
				return item != null ? Ok(item) : NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}
	}
}
