using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface IProductRepository
	{
		Task<Product?> GetByIdAsync(Guid id);
		Task<IEnumerable<Product>> GetAllAsync();
		//Task<Product?> GetByNameAsync(string productName);

		Task<IEnumerable<Product>> GetActiveProductsAsync();
		Task<Product> AddProductAsync(Product product);
		Task<Product> UpdateProductAsync(Guid id, Product product);
		Task<bool> UpdateQuantityAsync(Guid productId, int quantityChange);
		Task<bool> DeleteProductAsync(Guid id);
		// Thêm các method validation nếu cần
		Task<bool> ProductNameExistsAsync(string productName);

	}
}
