using ClothingStore.Application.DTOs.Product;
using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClothingStoreAppUI.Pages.Products
{
	public class EditModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		[BindProperty]
		public UpdateProductDTO Product { get; set; }
		[BindProperty(SupportsGet = true)]
		public Guid Id { get; set; } // Lấy ID từ route

		public List<SelectListItem> Categories { get; set; }
		public EditModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task<IActionResult> OnGet(Guid id)
		{
			if (id == Guid.Empty)
			{
				return RedirectToPage("/Products/List");
			}
			try
			{
				await LoadCategories(); // Thêm dòng này
				var client = _httpClientFactory.CreateClient();
				ProductResponseDTO productResponse = await client.GetFromJsonAsync<ProductResponseDTO>($"https://localhost:7170/api/Products/{id}");

				if (productResponse == null)
				{
					return RedirectToPage("/Products/List");
				}

				Product = new UpdateProductDTO
				{
					CategoryID = productResponse.CategoryID,
					ProductName = productResponse.ProductName,
					Description = productResponse.Description,
					Price = productResponse.Price,
					IsActive = productResponse.IsActive,
					ImageUrl = productResponse.ImageUrl
				};
				await LoadCategories(productResponse.CategoryID);

				return Page();
			}
			catch
			{
				return RedirectToPage("/Products/List");
			}
		}

		private async Task LoadCategories(Guid? currentCategoryId = null)
		{
			var client = _httpClientFactory.CreateClient();
			var categories = await client.GetFromJsonAsync<List<Category>>("https://localhost:7170/api/Categories");

			Categories = categories.Select(c => new SelectListItem
			{
				Value = c.CategoryId.ToString(),
				Text = c.CategoryName,
				Selected = (currentCategoryId != null && c.CategoryId == currentCategoryId)
			}).ToList();
		}


		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				await LoadCategories(Product.CategoryID);
				return Page();
			}

			try
			{
				var client = _httpClientFactory.CreateClient();

				var response = await client.PutAsJsonAsync(
					$"https://localhost:7170/api/Products/{Id}",
					Product);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Products/List");
				}
				else
				{
					// Xử lý lỗi chi tiết hơn
					var errorContent = await response.Content.ReadAsStringAsync();
					ModelState.AddModelError("", $"Update failed: {response.StatusCode} - {errorContent}");
					await LoadCategories(Product.CategoryID);
					return Page();
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Error updating product: {ex.Message}");
				await LoadCategories(Product.CategoryID);
				return Page();
			}
		}

		public async Task<IActionResult> OnPostDelete()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				var response = await client.DeleteAsync($"https://localhost:7170/api/Products/{Id}");

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Products/List");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					ModelState.AddModelError("", $"Delete failed: {response.StatusCode} - {errorContent}");
					await LoadCategories();
					return Page();
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Error deleting product: {ex.Message}");
				await LoadCategories();
				return Page();
			}
		}
	}
}
