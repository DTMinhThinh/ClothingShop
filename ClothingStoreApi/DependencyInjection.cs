using ClothingStore.Application;
using ClothingStore.Infrastructure;

namespace ClothingStoreApi
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddInfrastructureDI(configuration)
					.AddApplicationDI();
			return services;
		}
	}
}
