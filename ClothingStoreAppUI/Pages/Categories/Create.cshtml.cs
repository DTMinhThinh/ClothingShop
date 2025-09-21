using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Categories
{
	public class CreateModel : PageModel
	{

		private readonly ILogger<CreateModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;


		[BindProperty]
		public string CategoryName { get; set; }

		public string ErrorMessage { get; set; }

		public CreateModel(IHttpClientFactory httpClientFactory)
		{
			//_apiService = apiService;
			//_logger = logger;
			_httpClientFactory = httpClientFactory;
		}

		public void OnGet()
		{

		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (string.IsNullOrEmpty(CategoryName))
			{
				ErrorMessage = "Category is required";
				return Page();
			}

			try
			{
				var client = _httpClientFactory.CreateClient();

				Category category = new Category { CategoryName = CategoryName };

				var response = await client.PostAsJsonAsync("https://localhost:7170/api/Categories", category);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Categories/List");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					ErrorMessage = $"Lỗi: {response.StatusCode} - {errorContent}";
					return Page();
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"Lỗi kết nối: {ex.Message}";
				return Page();
			}
		}
	}
}
