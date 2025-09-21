using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries.OrderQueries
{
	public record GetOrdersByStatusQuery(OrderStatus Status) : IRequest<IEnumerable<OrderResponseDto>>;

	public class GetOrdersByStatusQueryHandler : IRequestHandler<GetOrdersByStatusQuery, IEnumerable<OrderResponseDto>>
	{
		private readonly IOrderRepository _orderRepository;

		public GetOrdersByStatusQueryHandler(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<IEnumerable<OrderResponseDto>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
		{
			var orders = await _orderRepository.GetByStatusAsync(request.Status);
			var result = new List<OrderResponseDto>();

			foreach (var order in orders)
			{
				var orderDetails = await _orderRepository.GetOrderDetailsAsync(order.OrderId);
				if (orderDetails != null)
					result.Add(orderDetails);
			}

			return result;
		}
	}
}
