using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries.CustomerQueries
{
	public record GetCustomerByIdQuery(Guid CustomerId) : IRequest<Customer>;
	public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByIdQuery, Customer>
	{
		public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
		{
			return await customerRepository.GetByIdAsync(request.CustomerId);
		}
	}
}
