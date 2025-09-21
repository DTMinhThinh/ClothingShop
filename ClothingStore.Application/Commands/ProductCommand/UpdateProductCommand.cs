using AutoMapper;
using ClothingStore.Application.DTOs.Product;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.ProductCommand
{
	public record UpdateProductCommand(
	Guid ProductId,
	UpdateProductDTO Dto) : IRequest<bool>;
	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}
		public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.ProductId);
			if (product == null)
				throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");

			// Update các field được phép theo cách thủ công
			product.CategoryID = request.Dto.CategoryID;
			product.ProductName = request.Dto.ProductName;
			product.Description = request.Dto.Description;
			product.Price = request.Dto.Price ?? 0;
			//product.IsActive = request.Dto.IsActive;
			product.ImageUrl = request.Dto.ImageUrl;

			var updatedProduct = await _productRepository.UpdateProductAsync(request.ProductId, product);
			return updatedProduct != null;
		}
	}
}
