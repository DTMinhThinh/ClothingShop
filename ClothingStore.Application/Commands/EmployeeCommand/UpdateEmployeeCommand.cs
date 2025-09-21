using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.EmployeeCommand
{
	public record UpdateEmployeeProfileCommand(
	   Guid EmployeeId,
	   string Name,
	   string? Email,
	   DateTime DateOfBirth,
	   string? Phone,
	   string? Address,
	   decimal Salary,
	   Guid? RoleID,
	   bool? IsActive = null
   ) : IRequest<Employee>;

	public class UpdateEmployeeProfileCommandHandler : IRequestHandler<UpdateEmployeeProfileCommand, Employee>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public UpdateEmployeeProfileCommandHandler(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task<Employee> Handle(UpdateEmployeeProfileCommand request, CancellationToken cancellationToken)
		{
			var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
			if (employee is null)
				throw new InvalidOperationException("Employee not found");

			// Cập nhật các trường cần thiết, không thay đổi Username/Password
			employee.Name = request.Name;
			employee.Email = request.Email;
			employee.DateOfBirth = request.DateOfBirth;
			employee.Phone = request.Phone;
			employee.Address = request.Address;
			employee.Salary = request.Salary;

			// Nếu có thay đổi trạng thái active
			if (request.IsActive.HasValue)
			{
				if (request.IsActive.Value) employee.Active();
				else employee.DeActive();
			}

			return await _employeeRepository.UpdateEmployeeAsync(employee.Id, employee);
		}
	}
}
