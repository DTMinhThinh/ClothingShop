namespace ClothingStore.Application.DTOs.OrderItem
{
	public class OrderItemCreateDTO
	{
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
