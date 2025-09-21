using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Roles
{
	public class ListModel : PageModel
	{
		private readonly ILogger<ListModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		[BindProperty]
		public List<Role> Roles { get; set; }
		public ListModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task OnGet()
		{
			var client = _httpClientFactory.CreateClient();
			var response = await client.GetAsync("https://localhost:7170/api/Roles");

			if (response.IsSuccessStatusCode)
			{
				Roles = await response.Content.ReadFromJsonAsync<List<Role>>(); ;
			}
			else
			{
				Roles = new List<Role>(); // Empty list in case of error
			}
		}
	}
}
