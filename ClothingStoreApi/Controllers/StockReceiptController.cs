using ClothingStore.Application.Commands.StockReceiptCommand;
using ClothingStore.Application.Queries.StockReceiptQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StockReceiptController : ControllerBase
	{
		private readonly IMediator _mediator;

		public StockReceiptController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpGet("")]
		public async Task<IActionResult> GetAllStockReceipts()
		{
			try
			{
				// Giả sử có query GetAllStockReceiptsQuery
				var receipts = await _mediator.Send(new GetAllStockReceiptsQuery());
				return Ok(receipts);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("{receiptId}")]
		public async Task<IActionResult> GetStockReceiptById([FromRoute] Guid receiptId)
		{
			try
			{
				// Giả sử có query GetStockReceiptByIdQuery
				var receipt = await _mediator.Send(new GetStockReceiptByIdQuery(receiptId));

				if (receipt == null)
					return NotFound(new { Error = "Stock receipt not found" });

				return Ok(receipt);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("employee/{employeeId}")]
		public async Task<IActionResult> GetStockReceiptsByEmployee([FromRoute] Guid employeeId)
		{
			try
			{
				// Giả sử có query GetStockReceiptsByEmployeeQuery
				var receipts = await _mediator.Send(new GetStockReceiptsByEmployeeQuery(employeeId));
				return Ok(receipts);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("date-range")]
		public async Task<IActionResult> GetStockReceiptsByDateRange(
			[FromQuery] DateTime startDate,
			[FromQuery] DateTime endDate)
		{
			try
			{
				// Giả sử có query GetStockReceiptsByDateRangeQuery
				var receipts = await _mediator.Send(new GetStockReceiptsByDateRangeQuery(startDate, endDate));
				return Ok(receipts);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpPost("")]
		public async Task<IActionResult> CreateStockReceipt([FromBody] CreateStockReceiptCommand command)
		{
			Console.WriteLine($"Received command: {JsonSerializer.Serialize(command)}");
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var result = await _mediator.Send(command);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
			}
		}

		//[HttpDelete("{receiptId}")]
		//public async Task<IActionResult> DeleteStockReceipt([FromRoute] Guid receiptId)
		//{
		//	try
		//	{
		//		// Giả sử có command DeleteStockReceiptCommand
		//		var result = await _mediator.Send(new DeleteStockReceiptCommand(receiptId));
		//		return result ? NoContent() : NotFound();
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { Error = ex.Message });
		//	}
		//}
	}
}
