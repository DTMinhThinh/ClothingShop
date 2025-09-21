using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.StockReceiptQueries
{
	public record GetAllStockReceiptsQuery : IRequest<IEnumerable<StockReceiptResponseDto>>;

	public class GetAllStockReceiptsQueryHandler : IRequestHandler<GetAllStockReceiptsQuery, IEnumerable<StockReceiptResponseDto>>
	{
		private readonly IStockReceiptRepository _stockReceiptRepository;

		public GetAllStockReceiptsQueryHandler(IStockReceiptRepository stockReceiptRepository)
		{
			_stockReceiptRepository = stockReceiptRepository;
		}

		public async Task<IEnumerable<StockReceiptResponseDto>> Handle(GetAllStockReceiptsQuery request, CancellationToken cancellationToken)
		{
			var receipts = await _stockReceiptRepository.GetAllAsync();
			var result = new List<StockReceiptResponseDto>();

			foreach (var receipt in receipts)
			{
				var details = await _stockReceiptRepository.GetReceiptDetailsAsync(receipt.ReceiptId);
				if (details != null)
					result.Add(details);
			}

			return result;
		}
	}
}
