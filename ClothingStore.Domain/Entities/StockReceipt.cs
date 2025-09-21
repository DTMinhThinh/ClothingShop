using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("StockReceipts")]
	public class StockReceipt
	{
		[Key]
		public Guid ReceiptId { get; private set; } = Guid.NewGuid();

		public Guid EmployeeId { get; set; }
		//public Employee Employee { get; set; }

		public DateTime DateCreated { get; set; }
		//public ICollection<StockReceiptItem>? StockReceiptItems { get; private set; }
		public StockReceipt()
		{

		}
	}
}
