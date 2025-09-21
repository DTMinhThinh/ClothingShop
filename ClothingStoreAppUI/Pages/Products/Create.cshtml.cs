using ClothingStore.Application.DTOs.Product;
using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClothingStoreAppUI.Pages.Products
{
	public class CreateModel : PageModel
	{
		[BindProperty]
		public CreateProductDTO product { get; set; }

		private readonly IHttpClientFactory _httpClientFactory;
		public List<SelectListItem> Categories { get; set; } // Danh sách categories cho dropdown
		string ErrorMessage { get; set; }
		public CreateModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;

		}
		public async Task OnGet()
		{
			await LoadCategories();
		}

		private async Task LoadCategories()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				var categories = await client.GetFromJsonAsync<List<Category>>("https://localhost:7170/api/Categories");

				Categories = categories?.Select(c => new SelectListItem
				{
					Value = c.CategoryId.ToString(),
					Text = c.CategoryName
				}).ToList() ?? new List<SelectListItem>();
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu cần
				Categories = new List<SelectListItem>();
				Console.WriteLine($"Error loading categories: {ex.Message}");
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			product.IsActive = true; // Mặc định sản phẩm mới luôn hoạt động
			if (string.IsNullOrEmpty(product.ProductName))
			{
				ErrorMessage = "Product name is required";
				return Page();
			}

			try
			{
				var client = _httpClientFactory.CreateClient();
				// LOG dữ liệu trước khi gửi
				Console.WriteLine($"Sending product data: {System.Text.Json.JsonSerializer.Serialize(product)}");
				var response = await client.PostAsJsonAsync("https://localhost:7170/api/Products", product);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Products/List"); // hoặc "/Products/List"
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
					ErrorMessage = $"Lỗi: {response.StatusCode} - {errorContent}";
					// Tải lại categories khi có lỗi
					await LoadCategories();
					return Page();
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"Error Connection: {ex.Message}";
				// Tải lại categories khi có lỗi
				await LoadCategories();
				return Page();
			}
		}
	}
}
