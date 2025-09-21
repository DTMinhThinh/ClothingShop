using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface IOrderItemRepository
	{
		Task AddAsync(OrderItem item);
		Task AddRangeAsync(IEnumerable<OrderItem> items);
		Task UpdateAsync(OrderItem item);
		Task DeleteAsync(OrderItem item);
		Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
		Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId);
		Task<OrderItem?> GetByIdAsync(Guid orderId, Guid productId);
	}
}
