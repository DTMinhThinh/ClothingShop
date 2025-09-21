using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries.EmployeeQueries
{
	public record GetAllEmployeesQuery() : IRequest<IEnumerable<Employee>>;

	public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<Employee>>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task<IEnumerable<Employee>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
		{
			return await _employeeRepository.GetAllAsync();
		}
	}
}
