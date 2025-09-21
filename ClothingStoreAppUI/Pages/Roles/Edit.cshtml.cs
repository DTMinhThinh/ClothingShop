using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClothingStoreAppUI.Pages.Roles
{
	public class EditModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		[BindProperty]
		public Role role { get; set; }
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
					RoleName = role.RoleName,
				};

				var response = await client.PutAsJsonAsync(
					$"https://localhost:7170/api/Roles/{Id}",
					updateData);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Roles/List");
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
				var response = await client.DeleteAsync($"https://localhost:7170/api/Roles/{Id}");

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Roles/List");
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
