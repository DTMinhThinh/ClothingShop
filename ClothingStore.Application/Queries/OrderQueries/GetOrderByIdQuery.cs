using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.Order
{
	public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderResponseDto>;

	public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponseDto>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IProductRepository _productRepository;

		public GetOrderByIdQueryHandler(
			IOrderRepository orderRepository,
			IOrderItemRepository orderItemRepository,
			ICustomerRepository customerRepository,
			IEmployeeRepository employeeRepository,
			IProductRepository productRepository)
		{
			_orderRepository = orderRepository;
			_orderItemRepository = orderItemRepository;
			_customerRepository = customerRepository;
			_employeeRepository = employeeRepository;
			_productRepository = productRepository;
		}

		public async Task<OrderResponseDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId);
			if (order == null) return null;

			var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
			var employee = order.EmployeeId.HasValue ?
				await _employeeRepository.GetByIdAsync(order.EmployeeId.Value) : null;

			var items = await _orderItemRepository.GetByOrderIdAsync(request.OrderId);
			var itemDtos = new List<OrderItemResponseDTO>();

			foreach (var item in items)
			{
				var product = await _productRepository.GetByIdAsync(item.ProductId);
				itemDtos.Add(new OrderItemResponseDTO
				{
					//OrderId = item.OrderId,
					ProductName = product?.ProductName ?? "Unknown",
					Quantity = item.Quantity,
					Price = item.Price,
					TotalPrice = item.Quantity * item.Price
				});
			}

			return new OrderResponseDto
			{
				OrderId = order.OrderId,
				Status = order.Status,
				DateCreated = order.DateCreated,
				DateShipped = order.DateShipped,
				TotalCost = order.TotalCost,
				CustomerName = customer?.Name ?? "Unknown",
				EmployeeName = employee?.Name,
				Items = itemDtos
			};
		}
	}
}
