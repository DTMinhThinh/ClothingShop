using ClothingStore.Application.Commands.OrderCommand;
using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.Queries.Order;
using ClothingStore.Application.Queries.OrderQueries;
using ClothingStore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IMediator _mediator;

		public OrderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

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
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOrders()
		{
			try
			{
				var orders = await _mediator.Send(new GetAllOrdersQuery());
				return Ok(orders);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
		{
			try
			{
				var order = await _mediator.Send(new GetOrderByIdQuery(orderId));
				return order != null ? Ok(order) : NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("customer/{customerId}")]
		public async Task<IActionResult> GetOrdersByCustomer([FromRoute] Guid customerId)
		{
			try
			{
				var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerId));
				return Ok(orders);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("employee/{employeeId}")]
		public async Task<IActionResult> GetOrdersByEmployee([FromRoute] Guid employeeId)
		{
			try
			{
				var orders = await _mediator.Send(new GetOrdersByEmployeeQuery(employeeId));
				return Ok(orders);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpGet("status/{status}")]
		public async Task<IActionResult> GetOrdersByStatus([FromRoute] OrderStatus status)
		{
			try
			{
				var orders = await _mediator.Send(new GetOrdersByStatusQuery(status));
				return Ok(orders);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpPatch("{orderId}/status")]
		public async Task<IActionResult> UpdateOrderStatus(
			[FromRoute] Guid orderId,
			[FromBody] UpdateOrderStatusDTO dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _mediator.Send(new UpdateOrderStatusCommand(orderId, dto.Status));
				return result ? Ok() : NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpDelete("{orderId}")]
		public async Task<IActionResult> DeleteOrder([FromRoute] Guid orderId)
		{
			try
			{
				var result = await _mediator.Send(new DeleteOrderCommand(orderId));
				return result ? NoContent() : NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}
	}
}
