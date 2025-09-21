using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.OrderQueries
{
	public record GetOrderItemsByProductIdQuery(Guid ProductId) : IRequest<IEnumerable<OrderItemResponseDTO>>;

	public class GetOrderItemsByProductIdQueryHandler : IRequestHandler<GetOrderItemsByProductIdQuery, IEnumerable<OrderItemResponseDTO>>
	{
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly IProductRepository _productRepository;

		public GetOrderItemsByProductIdQueryHandler(
			IOrderItemRepository orderItemRepository,
			IProductRepository productRepository)
		{
			_orderItemRepository = orderItemRepository;
			_productRepository = productRepository;
		}

		public async Task<IEnumerable<OrderItemResponseDTO>> Handle(GetOrderItemsByProductIdQuery request, CancellationToken cancellationToken)
		{
			var items = await _orderItemRepository.GetByProductIdAsync(request.ProductId);
			var result = new List<OrderItemResponseDTO>();

			foreach (var item in items)
			{
				var product = await _productRepository.GetByIdAsync(item.ProductId);
				result.Add(new OrderItemResponseDTO
				{
					ProductName = product?.ProductName ?? "Unknown",
					Quantity = item.Quantity,
					Price = item.Price,
					TotalPrice = item.Quantity * item.Price
				});
			}

			return result;
		}
	}
}
