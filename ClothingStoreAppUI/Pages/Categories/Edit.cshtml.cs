using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Categories
{
	public class EditModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		[BindProperty]
		public Category category { get; set; }
		[BindProperty(SupportsGet = true)]
		public Guid Id { get; set; } // Lấy ID từ route
		public EditModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				var client = _httpClientFactory.CreateClient();
				var updateData = new
				{
					CategoryName = category.CategoryName,
				};

				var response = await client.PutAsJsonAsync(
					$"https://localhost:7170/api/Categories/{Id}",
					updateData);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Categories/List");
				}
				else
				{
					// Xử lý lỗi chi tiết hơn
					var errorContent = await response.Content.ReadAsStringAsync();
					ModelState.AddModelError("", $"Update failed: {response.StatusCode} - {errorContent}");
					return Page();
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Error updating product: {ex.Message}");
				return Page();
			}
		}

		public async Task<IActionResult> OnPostDelete()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				var response = await client.DeleteAsync($"https://localhost:7170/api/Categories/{Id}");

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Categories/List");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					ModelState.AddModelError("", $"Delete failed: {response.StatusCode} - {errorContent}");
					return Page();
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Error deleting product: {ex.Message}");
				return Page();
			}
		}
	}
}
