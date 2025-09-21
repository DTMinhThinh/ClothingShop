using ClothingStore.Application.DTOs.StockReceiptItem;

namespace ClothingStore.Application.DTOs.StockReceipt
{
	public class StockReceiptResponseDto
	{
		public Guid ReceiptId { get; set; }
		public DateTime DateCreated { get; set; }

		// Chỉ giữ lại EmployeeName
		public string EmployeeName { get; set; }

		public List<StockReceiptItemResponseDTO> Items { get; set; } = new List<StockReceiptItemResponseDTO>();
		public decimal TotalAmount { get; set; }
	}
}
