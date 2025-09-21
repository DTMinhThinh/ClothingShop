using AutoMapper;
using ClothingStore.Application.Commands.EmployeeCommand;
using ClothingStore.Application.DTOs.Employee;
using ClothingStore.Application.Queries.EmployeeQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController(IMediator mediator, IMapper mapper) : ControllerBase
	{

		// Lấy tất cả Employees
		[HttpGet("")]
		public async Task<IActionResult> GetAllEmployeesAsync()
		{
			var result = await mediator.Send(new GetAllEmployeesQuery());
			return Ok(result);
		}

		// Lấy Employee theo ID
		[HttpGet("{EmployeeId}")]
		public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] Guid EmployeeId)
		{
			var result = await mediator.Send(new GetEmployeeByIdQuery(EmployeeId));
			return Ok(result);
		}

		// Cập nhật thông tin Employee
		[HttpPut("Profile/{EmployeeId}")]
		public async Task<IActionResult> UpdateEmployeeProfileAsync([FromRoute] Guid EmployeeId, [FromBody] UpdateProfileEmployeeDTO employeeDto)
		{
			// Gửi lệnh UpdateEmployeeProfileCommand đến Mediator
			var result = await mediator.Send(new UpdateEmployeeProfileCommand(
				EmployeeId,
				employeeDto.Name,
				employeeDto.Email,
				employeeDto.DateOfBirth,
				employeeDto.Phone,
				employeeDto.Address,
				employeeDto.Salary,
				employeeDto.RoleID
			));

			// Ánh xạ đối tượng Employee thành DTO và trả về
			var resultDto = mapper.Map<UpdateProfileEmployeeDTO>(result);
			return Ok(resultDto); // Trả về thông tin đã cập nhật
		}

		[HttpPost("")]
		public async Task<IActionResult> AddEmployeeAsync([FromBody] CreateEmployeeDTO employeeDto)
		{
			// Kiểm tra nếu có lỗi trong model binding
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Trả về lỗi nếu có lỗi binding
			}

			// Gửi lệnh AddEmployeeCommand đến Mediator
			var result = await mediator.Send(new AddEmployeeCommand(
				//employeeDto.Username,
				employeeDto.Password,
				employeeDto.Name,
				employeeDto.Email,
				employeeDto.DateOfBirth,
				employeeDto.Salary,
				employeeDto.Phone,
				employeeDto.Address,
				employeeDto.RoleID
			));

			return Ok(result); // Trả về ID của Employee vừa được tạo
		}


		// Xóa Employee theo ID
		[HttpDelete("{EmployeeId}")]
		public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid EmployeeId)
		{
			var result = await mediator.Send(new DeleteEmployeeCommand(EmployeeId));
			return result ? NoContent() : NotFound();
		}
	}
}

