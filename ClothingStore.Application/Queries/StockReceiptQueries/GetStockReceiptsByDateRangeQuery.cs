using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.StockReceiptQueries
{
	public record GetStockReceiptsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<StockReceiptResponseDto>>;

	public class GetStockReceiptsByDateRangeQueryHandler : IRequestHandler<GetStockReceiptsByDateRangeQuery, IEnumerable<StockReceiptResponseDto>>
	{
		private readonly IStockReceiptRepository _stockReceiptRepository;

		public GetStockReceiptsByDateRangeQueryHandler(IStockReceiptRepository stockReceiptRepository)
		{
			_stockReceiptRepository = stockReceiptRepository;
		}

		public async Task<IEnumerable<StockReceiptResponseDto>> Handle(GetStockReceiptsByDateRangeQuery request, CancellationToken cancellationToken)
		{
			var receipts = await _stockReceiptRepository.GetByDateRangeAsync(request.StartDate, request.EndDate);
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
