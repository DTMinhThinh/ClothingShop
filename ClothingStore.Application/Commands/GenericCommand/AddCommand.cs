using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.GenericCommand
{
	public record AddCommand<T>(T Entity) : IRequest<T> where T : class;
	public class AddCommandHandler<T>(IGenericRepository<T> repository) : IRequestHandler<AddCommand<T>, T> where T : class
	{
		public async Task<T> Handle(AddCommand<T> request, CancellationToken cancellationToken)
		{
			await repository.AddAsync(request.Entity);
			return request.Entity;
		}
	}
}
