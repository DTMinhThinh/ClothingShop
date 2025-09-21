using ClothingStore.Application.Commands.GenericCommand;
using ClothingStore.Application.DTOs.StockReceipt;
using ClothingStore.Application.Queries.GenericQueries;
using ClothingStore.Application.Queries.StockReceiptQueries;
using ClothingStore.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ClothingStore.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationDI(this IServiceCollection services)
		{
			// Đăng ký core MediatR
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssemblies(
					typeof(DependencyInjection).Assembly
				)
			);

			// Đăng ký cho từng entity bạn muốn dùng generic
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			RegisterGenericHandlersFor<Role>(services);
			RegisterGenericHandlersFor<Category>(services);

			services.AddTransient<IRequestHandler<GetAllStockReceiptsQuery, IEnumerable<StockReceiptResponseDto>>, GetAllStockReceiptsQueryHandler>();
			services.AddTransient<IRequestHandler<GetStockReceiptByIdQuery, StockReceiptResponseDto>, GetStockReceiptByIdQueryHandler>();

			return services;
		}

		private static void RegisterGenericHandlersFor<T>(IServiceCollection services) where T : class
		{
			// Add
			services.AddTransient<IRequestHandler<AddCommand<T>, T>, AddCommandHandler<T>>();
			// Delete
			services.AddTransient<IRequestHandler<DeleteCommand<T>, bool>, DeleteCommandHandler<T>>();
			// Update
			services.AddTransient<IRequestHandler<UpdateCommand<T>, T>, UpdateCommandHandler<T>>();
			// GetAll
			services.AddTransient<IRequestHandler<GetAllQuery<T>, IEnumerable<T>>, GetAllQueryHandler<T>>();
			// GetById (nhớ sửa handler trả T? như bên dưới)
			services.AddTransient<IRequestHandler<GetById<T>, T?>, GetByIdHandler<T>>();


		}
	}
}
