using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("Categories")]
	public class Category
	{
		[Key]
		public Guid CategoryId { get; set; } = Guid.NewGuid();
		public string CategoryName { get; set; }
		//public ICollection<Product>? Products { get; set; } = [];

		public Category()
		{

		}
		public Category(string name)
		{
			CategoryId = Guid.NewGuid();
			CategoryName = name;
		}
	}
}
