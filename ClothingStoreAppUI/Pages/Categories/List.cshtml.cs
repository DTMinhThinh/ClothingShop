using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Categories
{
	public class ListModel : PageModel
	{
		private readonly ILogger<ListModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		[BindProperty]
		public List<Category> Categories { get; set; }
		public ListModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task OnGet()
		{
			var client = _httpClientFactory.CreateClient();
			var response = await client.GetAsync("https://localhost:7170/api/Categories");

			if (response.IsSuccessStatusCode)
			{
				Categories = await response.Content.ReadFromJsonAsync<List<Category>>(); ;
			}
			else
			{
				Categories = new List<Category>(); // Empty list in case of error
			}
		}
	}
}
