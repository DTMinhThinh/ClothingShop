using AutoMapper;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.ProductCommand
{
	public record AddProductCommand(
	Guid CategoryID,
	string ProductName,
	string Description,
	decimal Price,
	bool IsActive,
	string ImageUrl) : IRequest<Guid>;

	public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Guid>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
		{
			// Tạo product entity từ command
			var product = new Product(
				productId: Guid.NewGuid(),
				categoryId: request.CategoryID,
				productName: request.ProductName,
				description: request.Description,
				price: request.Price,
				createDate: DateTime.UtcNow,
				isActived: request.IsActive,
				imageUrl: request.ImageUrl)
			{
				Quantity = 0
			};

			await _productRepository.AddProductAsync(product);
			return product.ProductId;
		}
	}
}
