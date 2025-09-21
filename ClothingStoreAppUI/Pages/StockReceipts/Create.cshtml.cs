using ClothingStore.Application.Commands.StockReceiptCommand;
using ClothingStore.Application.DTOs.Product;
using ClothingStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClothingStoreAppUI.Pages.StockReceipts
{
	public class CreateModel : PageModel
	{

		private readonly IHttpClientFactory _httpClientFactory;

		[BindProperty]
		public Guid EmployeeId { get; set; }

		[BindProperty]
		public List<StockReceiptItemCreateDto> Items { get; set; } = new List<StockReceiptItemCreateDto>();

		public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
		public List<SelectListItem> Products { get; set; } = new List<SelectListItem>(); // Thay đổi thành List<Product>

		public CreateModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task OnGet()
		{
			await LoadEmployees();
			await LoadProducts();
		}

		private async Task LoadEmployees()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				var employees = await client.GetFromJsonAsync<List<Employee>>("https://localhost:7170/api/Employees");
				Employees = employees?.Select(e => new SelectListItem
				{
					Value = e.Id.ToString(),
					Text = e.Name
				}).ToList() ?? new List<SelectListItem>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading employees: {ex.Message}");
				Employees = new List<SelectListItem>();
			}
		}

		private async Task LoadProducts()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				var response = await client.GetAsync("https://localhost:7170/api/Products");

				if (response.IsSuccessStatusCode)
				{
					// Sử dụng dynamic để đọc JSON response
					var products = await response.Content.ReadFromJsonAsync<List<ProductResponseDTO>>();

					Products = products?.Select(p => new SelectListItem
					{
						Value = p.ProductID.ToString(),  // Sử dụng ProductID từ DTO
						Text = $"{p.ProductName} (ID: {p.ProductID})"  // Hiển thị tên và ID
					}).ToList() ?? new List<SelectListItem>();

					// Debug: Kiểm tra dữ liệu
					Console.WriteLine($"Loaded {Products.Count} products");
					foreach (var product in Products.Take(3))
					{
						Console.WriteLine($"Value: {product.Value}, Text: {product.Text}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading products: {ex.Message}");
				Products = new List<SelectListItem>();
			}
		}

		//private async Task LoadProducts()
		//{
		//	try
		//	{
		//		var client = _httpClientFactory.CreateClient();
		//		var response = await client.GetAsync("https://localhost:7170/api/Products");

		//		if (response.IsSuccessStatusCode)
		//		{
		//			// Sử dụng ProductResponseDTO thay vì Product Entity
		//			var products = await response.Content.ReadFromJsonAsync<List<ProductResponseDTO>>();

		//			Products = products?.Select(p => new SelectListItem
		//			{
		//				Value = p.ProductID.ToString(),  // Sử dụng ProductId từ DTO
		//				Text = $"{p.ProductName} (ID: {p.ProductID})"
		//			}).ToList() ?? new List<SelectListItem>();
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error loading products: {ex.Message}");
		//		Products = new List<SelectListItem>();
		//	}
		//}
		public async Task<IActionResult> OnPost()
		{
			// Debug form data
			Console.WriteLine("=== FORM DATA ===");
			foreach (var key in Request.Form.Keys)
			{
				Console.WriteLine($"{key} = {Request.Form[key]}");
			}

			if (!ModelState.IsValid)
			{
				Console.WriteLine("=== MODELSTATE ERRORS ===");
				foreach (var key in ModelState.Keys)
				{
					foreach (var error in ModelState[key].Errors)
					{
						Console.WriteLine($"{key}: {error.ErrorMessage}");
					}
				}

				await LoadEmployees();
				await LoadProducts();
				return Page();
			}

			try
			{
				var client = _httpClientFactory.CreateClient();

				// Debug Items
				Console.WriteLine("=== ITEMS ===");
				Console.WriteLine($"Items count: {Items?.Count}");
				if (Items != null)
				{
					foreach (var item in Items)
					{
						Console.WriteLine($"ProductId: {item.ProductId}, Quantity: {item.Quantity}, UnitCost: {item.UnitCost}");
					}
				}

				var command = new
				{
					EmployeeId,
					Items = Items?.Select(item => new
					{
						item.ProductId,
						item.Quantity,
						item.UnitCost
					}).ToList()
				};

				var response = await client.PostAsJsonAsync("https://localhost:7170/api/StockReceipt", command);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToPage("/StockReceipts/List");
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"API Error: {errorContent}");
					ModelState.AddModelError("", $"Lỗi: {errorContent}");
					await LoadEmployees();
					await LoadProducts();
					return Page();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
				ModelState.AddModelError("", $"Lỗi kết nối: {ex.Message}");
				await LoadEmployees();
				await LoadProducts();
				return Page();
			}
		}
	}
}