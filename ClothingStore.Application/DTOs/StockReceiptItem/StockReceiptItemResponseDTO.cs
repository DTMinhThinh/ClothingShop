namespace ClothingStore.Application.DTOs.StockReceiptItem
{
	public class StockReceiptItemResponseDTO
	{
		public Guid ReceiptItemId { get; set; }
		public Guid ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal UnitCost { get; set; }
		public decimal TotalCost { get; set; }
	}
}
