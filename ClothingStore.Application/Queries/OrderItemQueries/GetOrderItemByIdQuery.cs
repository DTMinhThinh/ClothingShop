using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.OrderItemQueries
{
	public record GetOrderItemByIdQuery(Guid OrderId, Guid ProductId) : IRequest<OrderItemResponseDTO>;

	public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, OrderItemResponseDTO>
	{
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly IProductRepository _productRepository;

		public GetOrderItemByIdQueryHandler(
			IOrderItemRepository orderItemRepository,
			IProductRepository productRepository)
		{
			_orderItemRepository = orderItemRepository;
			_productRepository = productRepository;
		}

		public async Task<OrderItemResponseDTO> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
		{
			var item = await _orderItemRepository.GetByIdAsync(request.OrderId, request.ProductId);
			if (item == null)
				return null;

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
