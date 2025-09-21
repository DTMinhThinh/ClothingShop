using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.EmployeeCommand
{
	public record AddEmployeeCommand(
		//string Username,
		string Password,
		string Name,
		string Email,
		DateTime DateOfBirth,
		decimal Salary,
		string? Phone = null,
		string? Address = null,
		Guid? RoleID = null
	) : IRequest<Guid>;

	public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, Guid>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public AddEmployeeCommandHandler(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task<Guid> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
		{
			// Kiểm tra trùng username và email (application validation)
			//if (await _employeeRepository.GetByUsernameAsync(request.Username) != null)
			//	throw new InvalidOperationException("Username already exists");

			if (await _employeeRepository.GetByEmailAsync(request.Email) != null)
				throw new InvalidOperationException("Email already exists");

			// Tạo employee - domain validation sẽ chạy trong constructor
			var employee = new Employee(
				//request.Username,
				request.Password,
				request.Name,
				request.Email,
				request.DateOfBirth,
				request.Salary,
				request.RoleID,
				request.Phone,
				request.Address
			)
			{
				Phone = request.Phone,
				Address = request.Address
			};

			await _employeeRepository.AddEmployeeAsync(employee);
			return employee.Id;
		}
	}
}
