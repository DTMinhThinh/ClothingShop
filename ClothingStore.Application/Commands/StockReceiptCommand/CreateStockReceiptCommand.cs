using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using MediatR;

namespace ClothingStore.Application.Commands.StockReceiptCommand
{
	public record CreateStockReceiptCommand(
		Guid EmployeeId,
		List<StockReceiptItemCreateDto> Items
	) : IRequest<StockReceiptResponseDto>;

	public record StockReceiptItemCreateDto(
		Guid ProductId,
		int Quantity,
		decimal UnitCost
	);

	public class CreateStockReceiptCommandHandler : IRequestHandler<CreateStockReceiptCommand, StockReceiptResponseDto>
	{
		private readonly IStockReceiptRepository _stockReceiptRepository;
		private readonly IStockReceiptItemRepository _stockReceiptItemRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IProductRepository _productRepository;

		public CreateStockReceiptCommandHandler(
			IStockReceiptRepository stockReceiptRepository,
			IStockReceiptItemRepository stockReceiptItemRepository,
			IEmployeeRepository employeeRepository,
			IProductRepository productRepository)
		{
			_stockReceiptRepository = stockReceiptRepository;
			_stockReceiptItemRepository = stockReceiptItemRepository;
			_employeeRepository = employeeRepository;
			_productRepository = productRepository;
		}

		public async Task<StockReceiptResponseDto> Handle(CreateStockReceiptCommand request, CancellationToken cancellationToken)
		{
			// Validate employee exists
			var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
			if (employee == null)
				throw new InvalidOperationException("Employee not found");

			// Validate products exist
			foreach (var item in request.Items)
			{
				var product = await _productRepository.GetByIdAsync(item.ProductId);
				if (product == null)
					throw new InvalidOperationException($"Product with ID {item.ProductId} not found");
			}

			// Create receipt entity
			var receipt = new StockReceipt
			{
				EmployeeId = request.EmployeeId,
				DateCreated = DateTime.UtcNow
			};

			// Create receipt first
			var createdReceipt = await _stockReceiptRepository.CreateAsync(receipt);

			// Create items entities
			var stockReceiptItems = new List<StockReceiptItem>();
			foreach (var itemDto in request.Items)
			{
				var item = new StockReceiptItem
				{
					ReceiptItemId = Guid.NewGuid(),
					ReceiptId = createdReceipt.ReceiptId,
					ProductId = itemDto.ProductId,
					Quantity = itemDto.Quantity,
					UnitCost = itemDto.UnitCost
				};
				stockReceiptItems.Add(item);
			}

			// Add items using repository
			foreach (var item in stockReceiptItems)
			{
				await _stockReceiptItemRepository.AddAsync(item);
				await _productRepository.UpdateQuantityAsync(item.ProductId, item.Quantity);
			}

			// Get full receipt details
			var result = await _stockReceiptRepository.GetReceiptDetailsAsync(createdReceipt.ReceiptId);

			return result;
		}
	}
}
