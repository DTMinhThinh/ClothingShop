using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.StockReceiptItemCommand
{
	public class DeleteStockReceiptItemCommandHandler : IRequestHandler<DeleteStockReceiptItemCommand, bool>
	{
		private readonly IStockReceiptItemRepository _stockReceiptItemRepository;

		public DeleteStockReceiptItemCommandHandler(IStockReceiptItemRepository stockReceiptItemRepository)
		{
			_stockReceiptItemRepository = stockReceiptItemRepository;
		}

		public async Task<bool> Handle(DeleteStockReceiptItemCommand request, CancellationToken cancellationToken)
		{
			// Get item to delete
			var item = await _stockReceiptItemRepository.GetByIdAsync(request.ReceiptItemId);

			if (item == null)
				throw new InvalidOperationException("Stock receipt item not found");

			// Delete item
			await _stockReceiptItemRepository.DeleteAsync(item);

			return true;
		}
	}
}
