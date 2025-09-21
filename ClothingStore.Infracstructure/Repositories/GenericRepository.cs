using ClothingStore.Application.Interfaces;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ClothingStoreDBContext _dbContext;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(ClothingStoreDBContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}
		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T?> GetByIdAsync(Guid id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task UpdateAsync(Guid id, T entity)
		{
			var existingEntity = await _dbSet.FindAsync(id);
			if (existingEntity != null)
			{
				_dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
				await _dbContext.SaveChangesAsync();
			}
		}
	}
}
