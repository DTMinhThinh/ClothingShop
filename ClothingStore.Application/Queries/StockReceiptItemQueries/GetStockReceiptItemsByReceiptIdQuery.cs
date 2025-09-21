using ClothingStore.Application.DTOs.StockReceiptItem;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.StockReceiptItemQueries
{
	public record GetStockReceiptItemByIdQuery(Guid ReceiptItemId) : IRequest<StockReceiptItemResponseDTO>;
	public class GetStockReceiptItemsByReceiptIdQueryHandler : IRequestHandler<GetStockReceiptItemsByReceiptIdQuery, IEnumerable<StockReceiptItemResponseDTO>>
	{
		private readonly IStockReceiptItemRepository _stockReceiptItemRepository;
		private readonly IProductRepository _productRepository;

		public GetStockReceiptItemsByReceiptIdQueryHandler(
			IStockReceiptItemRepository stockReceiptItemRepository,
			IProductRepository productRepository)
		{
			_stockReceiptItemRepository = stockReceiptItemRepository;
			_productRepository = productRepository;
		}

		public async Task<IEnumerable<StockReceiptItemResponseDTO>> Handle(GetStockReceiptItemsByReceiptIdQuery request, CancellationToken cancellationToken)
		{
			var items = await _stockReceiptItemRepository.GetByReceiptIdAsync(request.ReceiptId);
			var result = new List<StockReceiptItemResponseDTO>();

			foreach (var item in items)
			{
				var product = await _productRepository.GetByIdAsync(item.ProductId);

				result.Add(new StockReceiptItemResponseDTO
				{
					ReceiptItemId = item.ReceiptItemId,
					ProductId = item.ProductId,
					ProductName = product?.ProductName ?? "Unknown",
					Quantity = item.Quantity,
					UnitCost = item.UnitCost,
					TotalCost = item.Quantity * item.UnitCost
				});
			}

			return result;
		}
	}
}
