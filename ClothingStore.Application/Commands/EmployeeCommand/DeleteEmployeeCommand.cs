using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.EmployeeCommand
{

	public record DeleteEmployeeCommand(Guid EmployeeId) : IRequest<bool>;

	public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
		{
			return await _employeeRepository.DeleteEmployeeAsync(request.EmployeeId);
		}
	}
}
