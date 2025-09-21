

namespace ClothingStoreAppUI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();

			// Add Blazor server-side services
			builder.Services.AddServerSideBlazor();

			// Configure HttpClient to call your existing API

			builder.Services.AddHttpClient("StoreApi", client =>
			{
				client.BaseAddress = new Uri("https://localhost:7000"); // Your API URL
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			});

			var app = builder.Build();



			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();
			app.MapGet("/", () => Results.Redirect("/Home"));
			app.Run();
		}
	}
}
