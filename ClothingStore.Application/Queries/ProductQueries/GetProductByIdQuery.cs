using ClothingStore.Application.DTOs.Product;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Queries.ProductQueries
{
	public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductResponseDTO>;

	public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDTO>
	{
		private readonly IProductRepository _productRepository;
		private readonly IGenericRepository<Category> _categoryRepository;

		public GetProductByIdQueryHandler(IProductRepository productRepository, IGenericRepository<Category> categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<ProductResponseDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.ProductId);
			if (product == null)
				throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");

			var category = await _categoryRepository.GetByIdAsync(product.CategoryID);

			return new ProductResponseDTO
			{
				ProductName = product.ProductName,
				CategoryID = product.CategoryID,
				Description = product.Description,
				Price = product.Price,
				Quantity = product.Quantity,
				IsActive = product.IsActive,
				ImageUrl = product.ImageUrl,
				CreateDate = product.CreateDate,
				CategoryName = category?.CategoryName ?? "Unknown"
			};
		}
	}
}
