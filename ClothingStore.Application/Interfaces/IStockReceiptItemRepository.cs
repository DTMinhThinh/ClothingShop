using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface IStockReceiptItemRepository
	{
		//Task AddRangeAsync(IEnumerable<StockReceiptItem> items);
		Task AddAsync(StockReceiptItem item);
		Task UpdateAsync(StockReceiptItem item);
		Task DeleteAsync(StockReceiptItem item);
		Task<IEnumerable<StockReceiptItem>> GetByReceiptIdAsync(Guid receiptId);
		Task<IEnumerable<StockReceiptItem>> GetByProductIdAsync(Guid productId);
		Task<StockReceiptItem?> GetByIdAsync(Guid receiptItemId);
	}
}
