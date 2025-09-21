using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Roles
{
	public class CreateModel : PageModel
	{

		private readonly ILogger<CreateModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;


		[BindProperty]
		public string RoleName { get; set; }

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
			if (string.IsNullOrEmpty(RoleName))
			{
				ErrorMessage = "Role Name is required";
				return Page();
			}

			try
			{
				var client = _httpClientFactory.CreateClient();

				Role role = new Role { RoleName = RoleName };

				var response = await client.PostAsJsonAsync("https://localhost:7170/api/Roles", role);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Categories/Index");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					ErrorMessage = $"L?i: {response.StatusCode} - {errorContent}";
					return Page();
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"L?i k?t n?i: {ex.Message}";
				return Page();
			}
		}
	}
}
