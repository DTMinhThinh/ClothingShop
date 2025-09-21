using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.OrderQueries
{
	public record GetOrdersByCustomerQuery(Guid CustomerId) : IRequest<IEnumerable<OrderResponseDto>>;

	public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderResponseDto>>
	{
		private readonly IOrderRepository _orderRepository;

		public GetOrdersByCustomerQueryHandler(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<IEnumerable<OrderResponseDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
		{
			return await _orderRepository.GetOrdersByCustomerAsync(request.CustomerId);
		}
	}
}
