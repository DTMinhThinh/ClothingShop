using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.ProductCommand
{
	public record DeleteProductCommand(Guid ProductId) : IRequest<bool>;

	public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
	{
		private readonly IProductRepository _productRepository;

		public DeleteProductCommandHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			return await _productRepository.DeleteProductAsync(request.ProductId);
		}
	}
}
