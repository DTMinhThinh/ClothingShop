using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class StockReceiptItemRepository : IStockReceiptItemRepository
	{
		private readonly ClothingStoreDBContext _context;

		public StockReceiptItemRepository(ClothingStoreDBContext context)
		{
			_context = context;
		}

		//public Task AddRangeAsync(IEnumerable<StockReceiptItem> items)
		//{
		//	await _context.StockReceiptItems.AddRangeAsync(items);
		//	await _context.SaveChangesAsync();
		//}

		public async Task<IEnumerable<StockReceiptItem>> GetByProductIdAsync(Guid productId)
		{
			return await _context.StockReceiptItems
			   .Where(item => item.ProductId == productId)
			   .ToListAsync();
		}

		public async Task<IEnumerable<StockReceiptItem>> GetByReceiptIdAsync(Guid receiptId)
		{
			return await _context.StockReceiptItems
				.Where(item => item.ReceiptId == receiptId)
				.ToListAsync();
		}

		public async Task AddAsync(StockReceiptItem item)
		{
			await _context.StockReceiptItems.AddAsync(item);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(StockReceiptItem item)
		{
			_context.StockReceiptItems.Update(item);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(StockReceiptItem item)
		{
			_context.StockReceiptItems.Remove(item);
			await _context.SaveChangesAsync();
		}
		public async Task<StockReceiptItem?> GetByIdAsync(Guid receiptItemId)
		{
			return await _context.StockReceiptItems
				.FirstOrDefaultAsync(item => item.ReceiptItemId == receiptItemId);
		}

		// Optional: Thêm phương thức để xóa trực tiếp bằng ID
		public async Task<bool> DeleteByIdAsync(Guid receiptItemId)
		{
			var item = await GetByIdAsync(receiptItemId);
			if (item != null)
			{
				_context.StockReceiptItems.Remove(item);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		// Optional: Thêm phương thức AddRange nếu cần
		public async Task AddRangeAsync(IEnumerable<StockReceiptItem> items)
		{
			await _context.StockReceiptItems.AddRangeAsync(items);
			await _context.SaveChangesAsync();
		}
	}
}
