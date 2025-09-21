using ClothingStore.Application.DTOs.Product;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries
{
	// Get All Products với CategoryName
	public record GetAllActiveProductsQuery() : IRequest<IEnumerable<ProductResponseDTO>>;

	public class GetAllProductsQueryHandler : IRequestHandler<GetAllActiveProductsQuery, IEnumerable<ProductResponseDTO>>
	{
		private readonly IProductRepository _productRepository;
		private readonly IGenericRepository<Category> _categoryRepository;

		public GetAllProductsQueryHandler(IProductRepository productRepository, IGenericRepository<Category> categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<IEnumerable<ProductResponseDTO>> Handle(GetAllActiveProductsQuery request, CancellationToken cancellationToken)
		{
			var products = await _productRepository.GetActiveProductsAsync();
			var result = new List<ProductResponseDTO>();

			foreach (var product in products)
			{
				var category = await _categoryRepository.GetByIdAsync(product.CategoryID);

				result.Add(new ProductResponseDTO
				{
					ProductID = product.ProductId,
					CategoryID = product.CategoryID,
					ProductName = product.ProductName,
					Description = product.Description,
					Price = product.Price,
					Quantity = product.Quantity,
					IsActive = product.IsActive,
					ImageUrl = product.ImageUrl,
					CreateDate = product.CreateDate,
					CategoryName = category?.CategoryName ?? "Unknown"
				});
			}

			return result;
		}
	}
}