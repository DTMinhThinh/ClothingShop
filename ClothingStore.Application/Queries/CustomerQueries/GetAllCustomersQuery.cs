using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries.CustomerQueries
{
	public record GetAllCustomersQuery() : IRequest<IEnumerable<Customer>>;
	public class GetAllCustomersQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
	{
		public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
		{
			return await customerRepository.GetAllAsync();
		}
		//}
	}
}
