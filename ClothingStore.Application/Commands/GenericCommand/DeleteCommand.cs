using ClothingStore.Application.Interfaces;
using MediatR;

namespace ClothingStore.Application.Commands.GenericCommand
{
	public record DeleteCommand<T>(Guid id) : IRequest<bool> where T : class;
	public class DeleteCommandHandler<T>(IGenericRepository<T> repository) : IRequestHandler<DeleteCommand<T>, bool> where T : class
	{
		public async Task<bool> Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
		{
			await repository.DeleteAsync(request.id);
			return true;
		}
	}
}
