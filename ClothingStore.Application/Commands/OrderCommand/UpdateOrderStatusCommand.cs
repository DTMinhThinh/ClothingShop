using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.OrderCommand
{
	public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest<bool>;

	public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
	{
		private readonly IOrderRepository _orderRepository;

		public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
		{
			return await _orderRepository.UpdateStatusAsync(request.OrderId, request.Status);
		}
	}
}
