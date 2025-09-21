using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("Orders")]
	public class Order
	{
		[Key]
		public Guid OrderId { get; set; }

		// Liên kết với Customer (Khách hàng)
		public Guid CustomerId { get; set; }
		//public virtual Customer Customer { get; private set; }

		// Liên kết với Employee (Nhân viên)
		public Guid? EmployeeId { get; set; }
		//public virtual Employee Employee { get; private set; }

		public OrderStatus Status { get; set; }

		public required DateTime DateCreated { get; set; }
		public required DateTime DateShipped { get; set; }

		public Decimal TotalCost { get; set; }

		//public ICollection<OrderItem> OrderItems { get; set; }  // Một Order có thể có nhiều OrderItem
	}

	public enum OrderStatus
	{
		Pending = 0,      // Đang chờ xử lý
		Processing = 1,   // Đang xử lý
		Shipped = 2,      // Đã giao hàng
		Completed = 3,    // Hoàn tất
		Canceled = 4      // Đã hủy
	}
}
