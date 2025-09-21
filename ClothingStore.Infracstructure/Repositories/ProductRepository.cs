using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ClothingStoreDBContext _context;

		public ProductRepository(ClothingStoreDBContext context)
		{
			_context = context;
		}

		public async Task<Product?> GetByIdAsync(Guid id)
		{
			return await _context.Products
				.FirstOrDefaultAsync(p => p.ProductId == id);
		}

		public async Task<Product?> GetByNameAsync(string productName)
		{
			return await _context.Products
				.FirstOrDefaultAsync(p => p.ProductName == productName);
		}

		public async Task<IEnumerable<Product>> GetAllAsync()
		{
			return await _context.Products
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
		{
			return await _context.Products
				.Where(p => p.CategoryID == categoryId)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetActiveProductsAsync()
		{
			return await _context.Products
				.Where(p => p.IsActive)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
		{
			return await _context.Products
				.Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsActive)
				.ToListAsync();
		}

		public async Task<Product> AddProductAsync(Product product)
		{
			await ValidateProductBeforeAddAsync(product);

			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return product;
		}

		public async Task<bool> UpdateQuantityAsync(Guid productId, int quantityChange)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				return false;

			// Kiểm tra nếu trừ quantity mà không đủ
			if (quantityChange < 0 && product.Quantity < Math.Abs(quantityChange))
				throw new InvalidOperationException($"Not enough stock for product {product.ProductName}");

			product.Quantity += quantityChange;
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
			return true;
		}
		private async Task ValidateProductBeforeAddAsync(Product product)
		{
			if (await _context.Products.AnyAsync(p => p.ProductName == product.ProductName))
				throw new InvalidOperationException("Product name already exists");

			if (string.IsNullOrWhiteSpace(product.ProductName))
				throw new ArgumentException("Product name is required");

			if (product.Price <= 0)
				throw new ArgumentException("Price must be greater than 0");
		}


		public async Task<Product> UpdateProductAsync(Guid id, Product product)
		{
			var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
			if (existingProduct is null)
				throw new KeyNotFoundException($"Product with ID {id} not found");

			// Cập nhật từng field
			existingProduct.ProductName = product.ProductName ?? existingProduct.ProductName;
			existingProduct.Description = product.Description ?? existingProduct.Description;
			existingProduct.Price = product.Price != 0 ? product.Price : existingProduct.Price;
			existingProduct.ImageUrl = product.ImageUrl ?? existingProduct.ImageUrl;
			existingProduct.IsActive = product.IsActive;

			// CategoryID có thể cần validation thêm
			if (product.CategoryID != Guid.Empty)
			{
				existingProduct.CategoryID = product.CategoryID;
			}

			await _context.SaveChangesAsync();
			return existingProduct;
		}

		public async Task<bool> DeleteProductAsync(Guid id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
			if (product != null)
			{
				// Soft delete (recommended) - set IsActive = false
				product.IsActive = false;

				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<bool> ProductNameExistsAsync(string productName)
		{
			return await _context.Products
				.AnyAsync(p => p.ProductName == productName);
		}

		public async Task<bool> ProductExistsAsync(Guid id)
		{
			return await _context.Products
				.AnyAsync(p => p.ProductId == id);
		}

		//public async Task UpdateProductQuantityAsync(Guid productId, int quantityChange)
		//{
		//	var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
		//	if (product != null)
		//	{
		//		// Đảm bảo quantity không âm
		//		var newQuantity = product.Quantity + quantityChange;
		//		if (newQuantity < 0)
		//			throw new InvalidOperationException("Insufficient product quantity");

		//		product.Quantity = newQuantity;
		//		await _context.SaveChangesAsync();
		//	}
		//}
	}
}
