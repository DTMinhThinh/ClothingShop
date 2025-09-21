using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.OrderCommand
{
	public record DeleteOrderCommand(Guid OrderId) : IRequest<bool>;

	public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderItemRepository _orderItemRepository;

		public DeleteOrderCommandHandler(
			IOrderRepository orderRepository,
			IOrderItemRepository orderItemRepository)
		{
			_orderRepository = orderRepository;
			_orderItemRepository = orderItemRepository;
		}

		public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId);
			if (order == null)
				return false;

			// Xóa các order items trước
			var items = await _orderItemRepository.GetByOrderIdAsync(request.OrderId);
			foreach (var item in items)
			{
				await _orderItemRepository.DeleteAsync(item);
			}

			// Xóa order
			return await _orderRepository.DeleteAsync(request.OrderId);
		}
	}
}
