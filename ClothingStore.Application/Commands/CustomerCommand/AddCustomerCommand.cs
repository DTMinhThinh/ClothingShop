using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.CustomerCommand
{
	public record AddCustomerCommand(
		//string Username,
		string Password,
		string Name,
		string Email,
		DateTime DateOfBirth,
		string? Phone = null,
		string? Address = null,
		string? BankAccountNumber = null) : IRequest<Guid>;

	public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Guid>
	{
		private readonly ICustomerRepository _customerRepository;

		public AddCustomerCommandHandler(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<Guid> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
		{
			// Tạo customer - domain validation sẽ chạy trong constructor
			var customer = new Customer(
				//request.Username,
				request.Password,
				request.Name,
				request.Email,
				request.DateOfBirth,
				request.Phone,
				request.Address,
				request.BankAccountNumber)
			;

			await _customerRepository.AddCustomerAsync(customer);
			return customer.Id;
		}
	}
}
