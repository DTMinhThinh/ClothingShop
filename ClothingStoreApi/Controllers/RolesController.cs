using ClothingStore.Application.Commands.GenericCommand;
using ClothingStore.Application.Queries.GenericQueries;
using ClothingStore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]


	public class RolesController(IMediator mediator) : ControllerBase
	{

		[HttpGet("")]
		public async Task<IActionResult> GetAllRolesAsync()
		{
			var result = await mediator.Send(new GetAllQuery<Role>());
			return Ok(result);
		}

		[HttpGet("{RoleId}")]
		public async Task<IActionResult> GetRoleByIdAsync([FromRoute] Guid RoleId)
		{
			var result = await mediator.Send(new GetById<Role>(RoleId));
			return Ok(result);
		}

		[HttpPut("{roleId}")]
		public async Task<IActionResult> UpdateRoleAsync([FromRoute] Guid roleId, [FromBody] Role role)
		{
			var result = await mediator.Send(new UpdateCommand<Role>(roleId, role));
			return Ok(result);
		}

		[HttpPost("")]
		public async Task<IActionResult> AddRoleAsync([FromBody] Role role)
		{
			var result = await mediator.Send(new AddCommand<Role>(role));
			return Ok(result);
		}

		[HttpDelete("{roleId}")]
		public async Task<IActionResult> DeleteRoleAsync([FromRoute] Guid roleId)
		{
			var result = await mediator.Send(new DeleteCommand<Role>(roleId));
			return result ? NoContent() : NotFound();
		}

	}
}
