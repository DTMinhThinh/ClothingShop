using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.GenericQueries
{
	public record GetById<T>(Guid id) : IRequest<T?> where T : class;
	public class GetByIdHandler<T>(IGenericRepository<T> repository) : IRequestHandler<GetById<T>, T?> where T : class
	{
		public async Task<T?> Handle(GetById<T> request, CancellationToken cancellationToken)
		{
			return await repository.GetByIdAsync(request.id);
		}
	}
}
