namespace ClothingStore.Application.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(Guid id);
		Task<IEnumerable<T>> GetAllAsync();
		Task AddAsync(T entity);
		Task UpdateAsync(Guid id, T entity);
		Task DeleteAsync(Guid id);
	}
}
