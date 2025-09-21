using ClothingStore.Application.DTOs.StockReceiptItem;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.StockReceiptItemCommand
{
	public class UpdateStockReceiptItemCommandHandler : IRequestHandler<UpdateStockReceiptItemCommand, StockReceiptItemResponseDTO>
	{
		private readonly IStockReceiptItemRepository _stockReceiptItemRepository;
		private readonly IProductRepository _productRepository;

		public UpdateStockReceiptItemCommandHandler(
			IStockReceiptItemRepository stockReceiptItemRepository,
			IProductRepository productRepository)
		{
			_stockReceiptItemRepository = stockReceiptItemRepository;
			_productRepository = productRepository;
		}

		public async Task<StockReceiptItemResponseDTO> Handle(UpdateStockReceiptItemCommand request, CancellationToken cancellationToken)
		{
			// Get existing item by ID
			var item = await _stockReceiptItemRepository.GetByIdAsync(request.ReceiptItemId);

			if (item == null)
				throw new InvalidOperationException("Stock receipt item not found");

			// Update properties
			item.Quantity = request.Quantity;
			item.UnitCost = request.UnitCost;

			// Save changes
			await _stockReceiptItemRepository.UpdateAsync(item);

			// Get product info for response
			var product = await _productRepository.GetByIdAsync(item.ProductId);

			return new StockReceiptItemResponseDTO
			{
				ReceiptItemId = item.ReceiptItemId,
				ProductId = item.ProductId,
				ProductName = product?.ProductName ?? "Unknown",
				Quantity = item.Quantity,
				UnitCost = item.UnitCost,
				TotalCost = item.Quantity * item.UnitCost
			};
		}
	}
}
