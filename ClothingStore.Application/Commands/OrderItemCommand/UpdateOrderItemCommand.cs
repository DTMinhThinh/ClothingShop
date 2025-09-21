using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.OrderItemCommand
{
	public record UpdateOrderItemCommand(
	   Guid OrderId,
	   Guid ProductId,
	   int Quantity,
	   decimal Price
   ) : IRequest<OrderItemResponseDTO>;

	public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, OrderItemResponseDTO>
	{
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly IProductRepository _productRepository;

		public UpdateOrderItemCommandHandler(
			IOrderItemRepository orderItemRepository,
			IProductRepository productRepository)
		{
			_orderItemRepository = orderItemRepository;
			_productRepository = productRepository;
		}

		public async Task<OrderItemResponseDTO> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
		{
			var item = await _orderItemRepository.GetByIdAsync(request.OrderId, request.ProductId);
			if (item == null)
				throw new InvalidOperationException("Order item not found");

			item.Quantity = request.Quantity;
			item.Price = request.Price;

			await _orderItemRepository.UpdateAsync(item);

			var product = await _productRepository.GetByIdAsync(item.ProductId);

			return new OrderItemResponseDTO
			{
				ProductName = product?.ProductName ?? "Unknown",
				Quantity = item.Quantity,
				Price = item.Price,
				TotalPrice = item.Quantity * item.Price
			};
		}
	}
}
