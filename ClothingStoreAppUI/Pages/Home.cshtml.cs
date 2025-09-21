using ClothingStore.Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages
{
	public class HomeModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public List<ProductResponseDTO> Products { get; set; } = new List<ProductResponseDTO>();

		public HomeModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task OnGet()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				var response = await client.GetAsync("https://localhost:7170/api/Products");

				if (response.IsSuccessStatusCode)
				{
					Products = await response.Content.ReadFromJsonAsync<List<ProductResponseDTO>>()
							  ?? new List<ProductResponseDTO>();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading products: {ex.Message}");
				Products = new List<ProductResponseDTO>();
			}
		}
	}
}
