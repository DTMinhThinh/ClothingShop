using ClothingStore.Application.DTOs.StockReceipt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.StockReceipts
{
	public class DetailsModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<DetailsModel> _logger;

		public StockReceiptResponseDto StockReceipt { get; set; }

		[BindProperty(SupportsGet = true)]
		public Guid Id { get; set; }

		public DetailsModel(IHttpClientFactory httpClientFactory, ILogger<DetailsModel> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task<IActionResult> OnGet()
		{
			if (Id == Guid.Empty)
			{
				return RedirectToPage("/StockReceipts/List");
			}

			try
			{
				var client = _httpClientFactory.CreateClient();

				// Gọi API để lấy chi tiết phiếu nhập kho
				var response = await client.GetAsync($"https://localhost:7170/api/StockReceipt/{Id}");

				if (response.IsSuccessStatusCode)
				{
					StockReceipt = await response.Content.ReadFromJsonAsync<StockReceiptResponseDto>();

					if (StockReceipt != null)
					{
						return Page();
					}
					else
					{
						StockReceipt = null;
						return Page();
					}
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					// Phiếu nhập không tồn tại
					StockReceipt = null;
					return Page();
				}
				else
				{
					_logger.LogError("Error getting stock receipt: {StatusCode}", response.StatusCode);
					TempData["ErrorMessage"] = "Lỗi khi tải chi tiết phiếu nhập kho";
					return RedirectToPage("/StockReceipts/List");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading stock receipt details");
				TempData["ErrorMessage"] = "Lỗi kết nối khi tải chi tiết phiếu nhập kho";
				return RedirectToPage("/StockReceipts/List");
			}
		}
	}
}
