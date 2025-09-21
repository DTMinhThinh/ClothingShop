using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.DTOs.StockReceiptItem;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class StockReceiptRepository : IStockReceiptRepository
	{
		private readonly ClothingStoreDBContext _context;
		private readonly IStockReceiptItemRepository _stockReceiptItemRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IProductRepository _productRepository;
		public StockReceiptRepository
			(
			ClothingStoreDBContext context,
			IStockReceiptItemRepository stockReceiptItemRepository,
			IEmployeeRepository employeeRepository,
			IProductRepository productRepository
			)
		{
			_context = context;
			_employeeRepository = employeeRepository;
			_productRepository = productRepository;
			_stockReceiptItemRepository = stockReceiptItemRepository;
		}

		public async Task<IEnumerable<StockReceipt>> GetAllAsync()
		{
			return await _context.StockReceipts.ToListAsync();
		}

		public async Task<IEnumerable<StockReceipt>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
		{
			return await _context.StockReceipts
				.Where(r => r.DateCreated >= startDate && r.DateCreated <= endDate)
				.ToListAsync();
		}

		public async Task<IEnumerable<StockReceipt>> GetByEmployeeAsync(Guid employeeId)
		{
			return await _context.StockReceipts
			   .Where(r => r.EmployeeId == employeeId)
			   .ToListAsync();
		}

		public async Task<StockReceipt?> GetByIdAsync(Guid receiptId)
		{
			return await _context.StockReceipts
			   .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);
		}

		public async Task<StockReceipt> CreateAsync(StockReceipt receipt)
		{
			await _context.StockReceipts.AddAsync(receipt);
			await _context.SaveChangesAsync();
			return receipt;
		}

		public async Task<StockReceiptResponseDto?> GetReceiptDetailsAsync(Guid receiptId)
		{
			var receipt = await GetByIdAsync(receiptId);
			if (receipt == null) return null;

			// Lấy thông tin employee từ EmployeeRepository
			var employee = await _employeeRepository.GetByIdAsync(receipt.EmployeeId);

			// Lấy items từ StockReceiptItemRepository
			var items = await _stockReceiptItemRepository.GetByReceiptIdAsync(receiptId);

			var itemDtos = new List<StockReceiptItemResponseDTO>();
			foreach (var item in items)
			{
				// Lấy thông tin product từ ProductRepository
				var product = await _productRepository.GetByIdAsync(item.ProductId);

				itemDtos.Add(new StockReceiptItemResponseDTO
				{
					ReceiptItemId = item.ReceiptItemId,
					ProductId = item.ProductId,
					ProductName = product?.ProductName ?? "Unknown Product",
					Quantity = item.Quantity,
					UnitCost = item.UnitCost,
					TotalCost = item.Quantity * item.UnitCost
				});
			}

			return new StockReceiptResponseDto
			{
				ReceiptId = receipt.ReceiptId,
				DateCreated = receipt.DateCreated,
				EmployeeName = employee?.Name ?? "Unknown Employee",
				Items = itemDtos,
				TotalAmount = itemDtos.Sum(i => i.TotalCost)
			};
		}

		// Phương thức để tạo receipt cùng với items (tiện ích)
		public async Task<StockReceiptResponseDto> CreateReceiptWithItemsAsync(StockReceipt receipt, List<StockReceiptItem> items)
		{
			// Tạo receipt
			var createdReceipt = await CreateAsync(receipt);

			// Thêm từng item
			foreach (var item in items)
			{
				item.ReceiptId = createdReceipt.ReceiptId;
				await _stockReceiptItemRepository.AddAsync(item);
			}

			// Trả về thông tin đầy đủ
			return await GetReceiptDetailsAsync(createdReceipt.ReceiptId);
		}
	}
}

