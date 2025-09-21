using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.OrderQueries
{
	public record GetAllOrdersQuery : IRequest<IEnumerable<OrderResponseDto>>;

	public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderResponseDto>>
	{
		private readonly IOrderRepository _orderRepository;

		public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<IEnumerable<OrderResponseDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
		{
			var orders = await _orderRepository.GetAllAsync();
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
