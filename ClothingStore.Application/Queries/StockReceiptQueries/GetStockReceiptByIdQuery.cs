using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.StockReceiptQueries
{
	public record GetStockReceiptByIdQuery(Guid ReceiptId) : IRequest<StockReceiptResponseDto>;

	public class GetStockReceiptByIdQueryHandler : IRequestHandler<GetStockReceiptByIdQuery, StockReceiptResponseDto>
	{
		private readonly IStockReceiptRepository _stockReceiptRepository;

		public GetStockReceiptByIdQueryHandler(IStockReceiptRepository stockReceiptRepository)
		{
			_stockReceiptRepository = stockReceiptRepository;
		}

		public async Task<StockReceiptResponseDto> Handle(GetStockReceiptByIdQuery request, CancellationToken cancellationToken)
		{
			return await _stockReceiptRepository.GetReceiptDetailsAsync(request.ReceiptId);
		}
	}
}
