using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.OrderQueries
{
	public record GetOrdersByEmployeeQuery(Guid EmployeeId) : IRequest<IEnumerable<OrderResponseDto>>;

	public class GetOrdersByEmployeeQueryHandler : IRequestHandler<GetOrdersByEmployeeQuery, IEnumerable<OrderResponseDto>>
	{
		private readonly IOrderRepository _orderRepository;

		public GetOrdersByEmployeeQueryHandler(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<IEnumerable<OrderResponseDto>> Handle(GetOrdersByEmployeeQuery request, CancellationToken cancellationToken)
		{
			var orders = await _orderRepository.GetByEmployeeIdAsync(request.EmployeeId);
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
