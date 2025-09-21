using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("OrderItems")]
	public class OrderItem
	{
		public Guid OrderId { get; set; }
		//public virtual Order Order { get; set; }

		// Khóa ngoại liên kết với Product
		public Guid ProductId { get; set; }
		//public virtual Product Product { get; set; }

		public int Quantity { get; set; }
		public decimal Price { get; set; }

		//public Order Order { get; set; }
		//public Product Product { get; set; }
	}
}

