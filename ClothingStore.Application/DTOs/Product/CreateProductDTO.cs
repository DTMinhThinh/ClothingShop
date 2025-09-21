using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Application.DTOs.Product
{
	public class CreateProductDTO
	{
		[Required]
		public Guid CategoryID { get; set; }
		[Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
		[StringLength(50, MinimumLength = 10, ErrorMessage = "Tên sản phẩm phải từ 10-50 ký tự")]
		public string ProductName { get; set; }
		[StringLength(300, ErrorMessage = "Mô tả không vượt quá 300 ký tự")]
		public string Description { get; set; }
		[Range(100000, 10000000, ErrorMessage = "Giá phải từ 100000 đến 10,000,000")]
		public decimal Price { get; set; }
		public bool IsActive { get; set; }
		[Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
		public string ImageUrl { get; set; }
	}
}
