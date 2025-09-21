using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.CustomerCommand
{
	public record DeleteCustomerCommand(Guid CustomerId) : IRequest<bool>;
	public class DeleteRoleCommandHandler(ICustomerRepository customerRepository) : IRequestHandler<DeleteCustomerCommand, bool>
	{
		public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
		{
			return await customerRepository.DeleteCustomerAsync(request.CustomerId);
		}
	}
}
