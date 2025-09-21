using ClothingStore.Application.DTOs.OrderItem;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.DTOs.Order
{
	public class OrderResponseDto
	{
		public Guid OrderId { get; set; }
		public OrderStatus Status { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateShipped { get; set; }
		public decimal TotalCost { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		public string? EmployeeName { get; set; }
		public List<OrderItemResponseDTO> Items { get; set; } = new List<OrderItemResponseDTO>();
	}
}
