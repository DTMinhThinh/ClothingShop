using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("Products")]
	public class Product
	{
		// Constructor không tham số cho Entity Framework
		public Product() { }

		// Constructor cho logic nghiệp vụ hoặc các trường hợp sử dụng khác
		public Product(Guid productId, Guid categoryId, string productName, string description, decimal price, DateTime createDate, bool isActived, string imageUrl)
		{
			ProductId = productId;
			CategoryID = categoryId;
			ProductName = productName;
			Description = description;
			Price = price;
			Quantity = 0;  // Mặc định Quantity là 0
			CreateDate = createDate;
			IsActive = isActived;
			ImageUrl = imageUrl;
		}

		// Auto-property với private set, bảo vệ không cho phép thay đổi sau khi khởi tạo
		// [
		[Key]
		public Guid ProductId { get; private set; }

		#region relation with category
		[Required]
		public Guid CategoryID { get; set; }
		//public virtual Category Category { get; set; }  // Không cần virtual nếu không dùng Lazy Loading
		#endregion
		public string ProductName { get; set; }
		public string Description { get; set; }

		// Auto-property cho Price với logic kiểm tra trong setter
		private decimal _price;
		public decimal Price
		{
			get { return _price; }
			set
			{
				if (value < 0)
					throw new ArgumentException("Price cannot be negative.");
				_price = value;
			}
		}

		public int Quantity { get; set; } = 0; // Mặc định 0 và không thể thay đổi ngoài constructor
		public DateTime CreateDate { get; set; }
		public bool IsActive { get; set; }
		public string ImageUrl { get; set; }

		// 1 Product có thể nằm trong nhiều ReceiptItem (qua nhiều phiếu nhập)
		//public ICollection<StockReceiptItem> ReceiptItems { get; set; } = new List<StockReceiptItem>();
	}
}
