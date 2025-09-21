using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.DTOs.Order
{
	public class UpdateOrderStatusDTO
	{
		public OrderStatus Status { get; set; }
	}
}
