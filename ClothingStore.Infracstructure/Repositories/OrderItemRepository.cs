using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class OrderItemRepository : IOrderItemRepository
	{
		private readonly ClothingStoreDBContext _context;

		public OrderItemRepository(ClothingStoreDBContext context)
		{
			_context = context;
		}

		public async Task AddAsync(OrderItem item)
		{
			await _context.OrderItems.AddAsync(item);
			await _context.SaveChangesAsync();
		}

		public async Task AddRangeAsync(IEnumerable<OrderItem> items)
		{
			await _context.OrderItems.AddRangeAsync(items);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(OrderItem item)
		{
			_context.OrderItems.Update(item);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(OrderItem item)
		{
			_context.OrderItems.Remove(item);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
		{
			return await _context.OrderItems
				.Where(oi => oi.OrderId == orderId)
				.ToListAsync();
		}

		public async Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId)
		{
			return await _context.OrderItems
				.Where(oi => oi.ProductId == productId)
				.ToListAsync();
		}

		public async Task<OrderItem?> GetByIdAsync(Guid orderId, Guid productId)
		{
			return await _context.OrderItems
				.FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);
		}
	}
}
