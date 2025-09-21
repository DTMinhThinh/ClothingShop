using ClothingStore.Application.DTOs.Order;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface IOrderRepository
	{
		Task<Order> CreateAsync(Order order);
		Task<Order?> GetByIdAsync(Guid orderId);
		Task<IEnumerable<Order>> GetAllAsync();
		//Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId);
		Task<IEnumerable<Order>> GetByEmployeeIdAsync(Guid employeeId);
		Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
		Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
		Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus status);
		Task<bool> UpdateAsync(Order order);
		Task<bool> DeleteAsync(Guid orderId);

		Task<OrderResponseDto?> GetOrderDetailsAsync(Guid orderId);
		Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerAsync(Guid customerId);
	}
}
