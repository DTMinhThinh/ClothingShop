using ClothingStore.Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Products
{
	public class ListModel : PageModel
	{
		private readonly ILogger<ListModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		[BindProperty]
		public List<ProductResponseDTO> Products { get; set; }
		public ListModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task OnGet()
		{
			var client = _httpClientFactory.CreateClient();
			var response = await client.GetAsync("https://localhost:7170/api/Products");

			if (response.IsSuccessStatusCode)
			{
				Products = await response.Content.ReadFromJsonAsync<List<ProductResponseDTO>>(); ;
			}
			else
			{
				Products = new List<ProductResponseDTO>(); // Empty list in case of error
			}
		}
	}
}
