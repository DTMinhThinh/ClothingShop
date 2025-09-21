using ClothingStore.Application.DTOs.OrderItem;

namespace ClothingStore.Application.DTOs.Order
{
	public class CreateOrderDTO
	{
		public string CustomerName { get; set; } = string.Empty;
		public string? EmployeeName { get; set; }
		public DateTime DateShipped { get; set; }
		public List<OrderItemCreateDTO> Items { get; set; } = new List<OrderItemCreateDTO>();
	}
}
