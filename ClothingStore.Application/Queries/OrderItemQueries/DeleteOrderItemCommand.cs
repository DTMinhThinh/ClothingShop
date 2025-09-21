using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.OrderItemQueries
{
	public record DeleteOrderItemCommand(Guid OrderId, Guid ProductId) : IRequest<bool>;

	public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, bool>
	{
		private readonly IOrderItemRepository _orderItemRepository;

		public DeleteOrderItemCommandHandler(IOrderItemRepository orderItemRepository)
		{
			_orderItemRepository = orderItemRepository;
		}

		public async Task<bool> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
		{
			var item = await _orderItemRepository.GetByIdAsync(request.OrderId, request.ProductId);
			if (item == null)
				return false;

			await _orderItemRepository.DeleteAsync(item);
			return true;
		}
	}
}
