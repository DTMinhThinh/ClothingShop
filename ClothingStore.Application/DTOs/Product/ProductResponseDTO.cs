using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClothingStore.Application.DTOs.Product
{
	public class ProductResponseDTO
	{
		[Required(ErrorMessage = "Product ID is required")]
		[JsonPropertyName("productID")]
		public Guid ProductID { get; set; }

		[Required(ErrorMessage = "Category ID is required")] // Thêm này
		[JsonPropertyName("categoryID")]
		public Guid CategoryID { get; set; }

		[Required(ErrorMessage = "Product name is required")]
		[StringLength(100, MinimumLength = 5, ErrorMessage = "Product name must be between 5-100 characters")]
		public string ProductName { get; set; }

		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Price is required")]
		[Range(10000, 1000000, ErrorMessage = "Price must be between 10000 and 1,000,000")]
		public decimal? Price { get; set; }

		[Range(0, 10000, ErrorMessage = "Quantity must be between 0 and 10,000")]
		public int Quantity { get; set; }

		public bool IsActive { get; set; }

		[Url(ErrorMessage = "Invalid URL format")]
		[StringLength(300, ErrorMessage = "Image URL cannot exceed 300 characters")]
		public string ImageUrl { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime CreateDate { get; set; }

		[Required(ErrorMessage = "Category name is required")]
		[StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
		public string CategoryName { get; set; }
	}
}
