using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.OrderCommand
{
	public record CreateOrderCommand(
	   Guid CustomerId,
	   Guid? EmployeeId,
	   DateTime DateShipped,
	   List<OrderItemCreateDTO> Items
   ) : IRequest<OrderResponseDto>;

	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponseDto>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IProductRepository _productRepository;

		public CreateOrderCommandHandler(
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

		public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
			// Validate customer exists
			var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
			if (customer == null)
				throw new InvalidOperationException("Customer not found");

			// Validate employee exists if provided
			if (request.EmployeeId.HasValue)
			{
				var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId.Value);
				if (employee == null)
					throw new InvalidOperationException("Employee not found");
			}

			// Validate products exist and calculate total cost
			decimal totalCost = 0;
			foreach (var item in request.Items)
			{
				var product = await _productRepository.GetByIdAsync(item.ProductId);  // SỬA LẠI item.ProductId


				if (product == null)
				{
					throw new InvalidOperationException($"Product with ID {item.ProductId} not found");
				}
				else if (product.Quantity < item.Quantity)
					throw new InvalidOperationException($"Not enough stock for product {product.ProductName}. Available: {product.Quantity}, Requested: {item.Quantity}");

				totalCost += item.Quantity * item.Price;
			}

			// Create order
			var order = new Order
			{
				CustomerId = request.CustomerId,
				EmployeeId = request.EmployeeId,
				Status = OrderStatus.Pending,
				DateCreated = DateTime.UtcNow,
				DateShipped = request.DateShipped,
				TotalCost = totalCost
			};


			var createdOrder = await _orderRepository.CreateAsync(order);

			// Create order items
			foreach (var itemDto in request.Items)
			{
				var orderItem = new OrderItem
				{
					OrderId = createdOrder.OrderId,
					ProductId = itemDto.ProductId,  // SỬA LẠI itemDto.ProductId
					Quantity = itemDto.Quantity,
					Price = itemDto.Price
				};
				await _orderItemRepository.AddAsync(orderItem);
				await _productRepository.UpdateQuantityAsync(itemDto.ProductId, -itemDto.Quantity);
			}

			// Return order details từ repository
			return await _orderRepository.GetOrderDetailsAsync(createdOrder.OrderId);
		}
	}
}
