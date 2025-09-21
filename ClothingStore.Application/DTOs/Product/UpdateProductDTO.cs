using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Application.DTOs.Product
{
	public class UpdateProductDTO
	{

		[Required(ErrorMessage = "Category is required")]
		public Guid CategoryID { get; set; }

		[Required(ErrorMessage = "Product name is required")]
		[StringLength(50, MinimumLength = 10, ErrorMessage = "Product name must be between 2 and 100 characters")]
		public string ProductName { get; set; }

		[StringLength(300, ErrorMessage = "Description cannot exceed 300 characters")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "Price is required")]
		[Range(10000, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
		public decimal? Price { get; set; }

		public bool IsActive { get; set; }

		[Url(ErrorMessage = "Invalid URL format")]
		public string? ImageUrl { get; set; }
	}
}
