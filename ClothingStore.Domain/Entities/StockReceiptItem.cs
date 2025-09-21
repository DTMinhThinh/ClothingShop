using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("StockReceiptItems")]
	public class StockReceiptItem
	{
		[Key]
		public Guid ReceiptItemId { get; set; }  // Khóa chính của ReceiptItem
		public Guid ReceiptId { get; set; }  // FK đến StockReceipt
		public Guid ProductId { get; set; } // FK đến Product

		public int Quantity { get; set; }

		public Decimal UnitCost { get; set; }

		public StockReceiptItem()
		{

		}
		// Mối quan hệ với Cart
		//public StockReceipt StockReceipt { get; set; }

		// Mối quan hệ với Product
		//public Product Product { get; set; }
	}
}
