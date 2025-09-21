using ClothingStore.Application.Interfaces;
using ClothingStore.Infrastructure.Data;
using ClothingStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClothingStore.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ClothingStoreDBContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
					.EnableSensitiveDataLogging()
					.EnableDetailedErrors();
			});
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IStockReceiptItemRepository, StockReceiptItemRepository>();
			services.AddScoped<IStockReceiptRepository, StockReceiptRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IOrderItemRepository, OrderItemRepository>();

			return services;
		}
	}
}
