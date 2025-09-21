using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface IStockReceiptRepository
	{
		Task<StockReceipt> CreateAsync(StockReceipt receipt);
		Task<StockReceiptResponseDto> CreateReceiptWithItemsAsync(StockReceipt receipt, List<StockReceiptItem> items);

		Task<StockReceiptResponseDto?> GetReceiptDetailsAsync(Guid receiptId);
		Task<StockReceipt?> GetByIdAsync(Guid receiptId);
		Task<IEnumerable<StockReceipt>> GetAllAsync();
		Task<IEnumerable<StockReceipt>> GetByEmployeeAsync(Guid employeeId);
		Task<IEnumerable<StockReceipt>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	}
}
