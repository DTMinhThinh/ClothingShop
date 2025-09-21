using ClothingStore.Domain.Entities;

namespace ClothingStore.Infrastructure.Repositories
{
	public interface ITokenService
	{
		string CreateToken(User user);
	}
}
