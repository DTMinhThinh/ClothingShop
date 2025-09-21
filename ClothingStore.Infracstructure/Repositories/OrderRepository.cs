using ClothingStore.Application.DTOs.Order;
using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ClothingStoreDBContext _context;
		private readonly ICustomerRepository _customerRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IProductRepository _productRepository;
		private readonly IOrderItemRepository _orderItemRepository;


		public OrderRepository
			(
			ClothingStoreDBContext context,
			ICustomerRepository customerRepository,
			IEmployeeRepository employeeRepository,
			IProductRepository productRepository,
			IOrderItemRepository orderItemRepository
			)
		{
			_context = context;
			_customerRepository = customerRepository;
			_employeeRepository = employeeRepository;
			_productRepository = productRepository;
			_orderItemRepository = orderItemRepository;
		}

		public async Task<Order> CreateAsync(Order order)
		{
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
			return order;
		}

		public async Task<Order?> GetByIdAsync(Guid orderId)
		{
			return await _context.Orders
				.FirstOrDefaultAsync(o => o.OrderId == orderId);
		}

		public async Task<IEnumerable<Order>> GetAllAsync()
		{
			return await _context.Orders.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId)
		{
			return await _context.Orders
				.Where(o => o.CustomerId == customerId)
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetByEmployeeIdAsync(Guid employeeId)
		{
			return await _context.Orders
				.Where(o => o.EmployeeId == employeeId)
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
		{
			return await _context.Orders
				.Where(o => o.Status == status)
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
		{
			return await _context.Orders
				.Where(o => o.DateCreated >= startDate && o.DateCreated <= endDate)
				.ToListAsync();
		}

		public async Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus status)
		{
			var order = await GetByIdAsync(orderId);
			if (order == null)
				return false;

			order.Status = status;
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateAsync(Order order)
		{
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(Guid orderId)
		{
			var order = await GetByIdAsync(orderId);
			if (order == null)
				return false;

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<OrderResponseDto?> GetOrderDetailsAsync(Guid orderId)
		{
			var order = await GetByIdAsync(orderId);
			if (order == null) return null;

			var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
			var employee = order.EmployeeId.HasValue ?
				await _employeeRepository.GetByIdAsync(order.EmployeeId.Value) : null;

			var items = await _orderItemRepository.GetByOrderIdAsync(orderId);
			var itemDtos = new List<OrderItemResponseDTO>();

			foreach (var item in items)
			{
				var product = await _productRepository.GetByIdAsync(item.ProductId);
				itemDtos.Add(new OrderItemResponseDTO
				{
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

		public async Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerAsync(Guid customerId)
		{
			var orders = await GetByCustomerIdAsync(customerId);
			var result = new List<OrderResponseDto>();

			foreach (var order in orders)
			{
				var orderDetails = await GetOrderDetailsAsync(order.OrderId);
				if (orderDetails != null)
					result.Add(orderDetails);
			}

			return result;
		}
	}
}
