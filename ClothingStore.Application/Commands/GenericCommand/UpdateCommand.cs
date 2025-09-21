using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.GenericCommand
{
	public record UpdateCommand<T>(Guid id, T Entity) : IRequest<T> where T : class;
	public class UpdateCommandHandler<T>(IGenericRepository<T> repository) : IRequestHandler<UpdateCommand<T>, T> where T : class
	{
		public async Task<T> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
		{
			await repository.UpdateAsync(request.id, request.Entity);
			return request.Entity;
		}
	}
}
