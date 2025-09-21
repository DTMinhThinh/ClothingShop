using ClothingStore.Application.DTOs.Customer;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.CustomerCommand
{
	public record UpdateCustomerProfileCommand(
	Guid CustomerId,
	UpdateProfileCustomerDTO Dto) : IRequest<Customer>;
	public class UpdateCustomerCommandHandle(ICustomerRepository customerRepository) : IRequestHandler<UpdateCustomerProfileCommand, Customer>
	{
		public async Task<Customer> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
		{
			// Repository sẽ handle all validation và update
			return await customerRepository.UpdateCustomerAsync(request.CustomerId, request.Dto);
		}
	}
}
