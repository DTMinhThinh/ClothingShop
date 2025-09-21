using ClothingStore.Application.Commands.StockReceiptItemCommand;
using ClothingStore.Application.Queries;
using ClothingStore.Application.Queries.StockReceiptItemQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StockReceiptItemController : ControllerBase
	{
		private readonly IMediator _mediator;

		public StockReceiptItemController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("receipt/{receiptId}")]
		public async Task<IActionResult> GetItemsByReceiptId([FromRoute] Guid receiptId)
		{
			try
			{
				var items = await _mediator.Send(new GetStockReceiptItemsByReceiptIdQuery(receiptId));
				return Ok(items);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("{receiptItemId}")]
		public async Task<IActionResult> GetStockReceiptItemById([FromRoute] Guid receiptItemId)
		{
			try
			{
				var item = await _mediator.Send(new GetStockReceiptItemByIdQuery(receiptItemId));

				if (item == null)
					return NotFound(new { Error = "Stock receipt item not found" });

				return Ok(item);
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
				var items = await _mediator.Send(new GetStockReceiptItemsByProductIdQuery(productId));
				return Ok(items);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpPut("{receiptItemId}")]
		public async Task<IActionResult> UpdateStockReceiptItem(
			[FromRoute] Guid receiptItemId,
			[FromBody] UpdateStockReceiptItemCommand command)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _mediator.Send(command with { ReceiptItemId = receiptItemId });
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

		[HttpDelete("{receiptItemId}")]
		public async Task<IActionResult> DeleteStockReceiptItem([FromRoute] Guid receiptItemId)
		{
			try
			{
				var result = await _mediator.Send(new DeleteStockReceiptItemCommand(receiptItemId));
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
	}
}
