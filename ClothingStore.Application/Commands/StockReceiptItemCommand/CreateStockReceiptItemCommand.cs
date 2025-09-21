using ClothingStore.Application.DTOs.StockReceiptItem;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.StockReceiptItemCommand
{
	public record AddStockReceiptItemCommand(
	   Guid ReceiptId,
	   Guid ProductId,
	   int Quantity,
	   decimal UnitCost
   ) : IRequest<StockReceiptItemResponseDTO>;

	public record UpdateStockReceiptItemCommand(
		Guid ReceiptItemId,
		int Quantity,
		decimal UnitCost
	) : IRequest<StockReceiptItemResponseDTO>;

	public record DeleteStockReceiptItemCommand(
		Guid ReceiptItemId
	) : IRequest<bool>;

	public class AddStockReceiptItemCommandHandler : IRequestHandler<AddStockReceiptItemCommand, StockReceiptItemResponseDTO>
	{
		private readonly IStockReceiptItemRepository _stockReceiptItemRepository;
		private readonly IStockReceiptRepository _stockReceiptRepository;
		private readonly IProductRepository _productRepository;

		public AddStockReceiptItemCommandHandler(
			IStockReceiptItemRepository stockReceiptItemRepository,
			IStockReceiptRepository stockReceiptRepository,
			IProductRepository productRepository)
		{
			_stockReceiptItemRepository = stockReceiptItemRepository;
			_stockReceiptRepository = stockReceiptRepository;
			_productRepository = productRepository;
		}

		public async Task<StockReceiptItemResponseDTO> Handle(AddStockReceiptItemCommand request, CancellationToken cancellationToken)
		{
			// Validate receipt exists
			var receipt = await _stockReceiptRepository.GetByIdAsync(request.ReceiptId);
			if (receipt == null)
				throw new InvalidOperationException("Receipt not found");

			// Validate product exists
			var product = await _productRepository.GetByIdAsync(request.ProductId);
			if (product == null)
				throw new InvalidOperationException("Product not found");

			// Create item
			var item = new StockReceiptItem
			{
				ReceiptItemId = Guid.NewGuid(),
				ReceiptId = request.ReceiptId,
				ProductId = request.ProductId,
				Quantity = request.Quantity,
				UnitCost = request.UnitCost
			};

			await _stockReceiptItemRepository.AddAsync(item);

			return new StockReceiptItemResponseDTO
			{
				ReceiptItemId = item.ReceiptItemId,
				ProductId = item.ProductId,
				ProductName = product.ProductName,
				Quantity = item.Quantity,
				UnitCost = item.UnitCost,
				TotalCost = item.Quantity * item.UnitCost
			};
		}
	}
}
