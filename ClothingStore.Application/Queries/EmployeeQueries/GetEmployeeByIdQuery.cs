using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries.EmployeeQueries
{
	public record GetEmployeeByIdQuery(Guid EmployeeId) : IRequest<Employee>;

	public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
		{
			return await _employeeRepository.GetByIdAsync(request.EmployeeId);
		}
	}
}
