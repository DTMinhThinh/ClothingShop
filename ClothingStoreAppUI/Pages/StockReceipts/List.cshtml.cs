using ClothingStore.Application.DTOs.StockReceipt;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.StockReceipts
{
	public class ListModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<ListModel> _logger;

		public List<StockReceiptResponseDto> StockReceipts { get; set; } = new List<StockReceiptResponseDto>();

		public ListModel(IHttpClientFactory httpClientFactory, ILogger<ListModel> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task OnGet()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();

				// Gọi API để lấy danh sách phiếu nhập kho
				var response = await client.GetAsync("https://localhost:7170/api/StockReceipt");

				if (response.IsSuccessStatusCode)
				{
					StockReceipts = await response.Content.ReadFromJsonAsync<List<StockReceiptResponseDto>>()
								   ?? new List<StockReceiptResponseDto>();
				}
				else
				{
					_logger.LogError("Error getting stock receipts: {StatusCode}", response.StatusCode);
					TempData["ErrorMessage"] = "Lỗi khi tải danh sách phiếu nhập kho";
					StockReceipts = new List<StockReceiptResponseDto>();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading stock receipts list");
				TempData["ErrorMessage"] = "Lỗi kết nối khi tải danh sách phiếu nhập kho";
				StockReceipts = new List<StockReceiptResponseDto>();
			}
		}
	}
}
