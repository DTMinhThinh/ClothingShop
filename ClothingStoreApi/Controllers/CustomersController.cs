using AutoMapper;
using ClothingStore.Application.Commands.CustomerCommand;
using ClothingStore.Application.DTOs.Customer;
using ClothingStore.Application.Queries.CustomerQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController(IMediator mediator, IMapper mapper) : ControllerBase
	{

		[HttpGet("")]
		public async Task<IActionResult> GetAllCustomersAsync()
		{
			var result = await mediator.Send(new GetAllCustomersQuery());
			return Ok(result);
		}

		[HttpGet("{CustomerId}")]
		public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] Guid CustomerId)
		{
			var result = await mediator.Send(new GetCustomerByIdQuery(CustomerId));
			return Ok(result);
		}

		[HttpPut("Profile/{CustomerId}")]
		public async Task<IActionResult> UpdateProfileCustomerAsync([FromRoute] Guid CustomerId, [FromBody] UpdateProfileCustomerDTO customer)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await mediator.Send(new UpdateCustomerProfileCommand(CustomerId, customer));
				return Ok(new
				{
					Message = "Update successful",
					Data = customer
				});
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { Error = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(new { Error = ex.Message });
			}
		}

		[HttpPost("")]
		public async Task<IActionResult> AddCustomerAsync([FromBody] CreateCustomerDTO customerDto)
		{
			// Kiểm tra nếu có lỗi trong model binding
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Trả về lỗi nếu có lỗi binding
			}
			try
			{
				var result = await mediator.Send(new AddCustomerCommand(
					//customerDto.Username,
					customerDto.Password,
					customerDto.Name,
					customerDto.Email,
					customerDto.DateOfBirth,
					customerDto.Phone,
					customerDto.Address,
					customerDto.BankAccountNumber
				));

				return Ok(result);
			}
			catch (InvalidOperationException ex) // Từ repository validation
			{
				return Conflict(new { Error = ex.Message });
			}
			catch (ArgumentException ex) // Từ repository validation
			{
				return BadRequest(new { Error = ex.Message });
			}

		}

		[HttpDelete("{CustomerId}")]
		public async Task<IActionResult> DeleteRoleAsync([FromRoute] Guid CustomerId)
		{
			var result = await mediator.Send(new DeleteCustomerCommand(CustomerId));
			return result ? NoContent() : NotFound();
		}
	}
}
