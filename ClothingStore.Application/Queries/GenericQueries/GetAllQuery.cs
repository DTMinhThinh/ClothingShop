using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Queries.GenericQueries
{
	public record GetAllQuery<T>() : IRequest<IEnumerable<T>> where T : class;
	public class GetAllQueryHandler<T>(IGenericRepository<T> repository) : IRequestHandler<GetAllQuery<T>, IEnumerable<T>> where T : class
	{
		public async Task<IEnumerable<T>> Handle(GetAllQuery<T> request, CancellationToken cancellationToken)
		{
			return await repository.GetAllAsync();
		}
	}
}
