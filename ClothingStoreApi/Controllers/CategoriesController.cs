using ClothingStore.Application.Commands.GenericCommand;
using ClothingStore.Application.Queries.GenericQueries;
using ClothingStore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController(IMediator mediator) : ControllerBase
	{

		[HttpGet("")]
		public async Task<IActionResult> GetAllCategoriesAsync()
		{
			var result = await mediator.Send(new GetAllQuery<Category>());
			return Ok(result);
		}


		[HttpGet("{CategoryId}")]
		public async Task<IActionResult> GetCategoryByIdAsync([FromRoute] Guid CategoryId)
		{
			var result = await mediator.Send(new GetById<Category>(CategoryId));
			return Ok(result);
		}



		[HttpPost("")]
		public async Task<IActionResult> AddCategoryAsync([FromBody] Category category)
		{
			var result = await mediator.Send(new AddCommand<Category>(category));
			return Ok(result);
		}

		[HttpPut("{CategoryId}")]
		public async Task<IActionResult> UpdateCategoryAsync([FromRoute] Guid CategoryId, [FromBody] Category category)
		{
			//var result = await mediator.Send(new UpdateCommand<Category>(CategoryId, category));
			//return Ok(result);
			try
			{
				// Lấy category hiện tại
				var existingCategory = await mediator.Send(new GetById<Category>(CategoryId));
				if (existingCategory == null)
					return NotFound();

				// Chỉ update các field được phép
				existingCategory.CategoryName = category.CategoryName;
				// Gọi command để update
				var result = await mediator.Send(new UpdateCommand<Category>(CategoryId, existingCategory));
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Error = ex.Message });
			}
		}

		[HttpDelete("{CategoryId}")]
		public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid CategoryId)
		{
			var result = await mediator.Send(new DeleteCommand<Category>(CategoryId));
			return result ? NoContent() : NotFound();
		}
	}
}
