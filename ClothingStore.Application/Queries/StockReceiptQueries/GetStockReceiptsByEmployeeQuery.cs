using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.StockReceiptQueries
{
	public record GetStockReceiptsByEmployeeQuery(Guid EmployeeId) : IRequest<IEnumerable<StockReceiptResponseDto>>;

	public class GetStockReceiptsByEmployeeQueryHandler : IRequestHandler<GetStockReceiptsByEmployeeQuery, IEnumerable<StockReceiptResponseDto>>
	{
		private readonly IStockReceiptRepository _stockReceiptRepository;

		public GetStockReceiptsByEmployeeQueryHandler(IStockReceiptRepository stockReceiptRepository)
		{
			_stockReceiptRepository = stockReceiptRepository;
		}

		public async Task<IEnumerable<StockReceiptResponseDto>> Handle(GetStockReceiptsByEmployeeQuery request, CancellationToken cancellationToken)
		{
			var receipts = await _stockReceiptRepository.GetByEmployeeAsync(request.EmployeeId);
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
