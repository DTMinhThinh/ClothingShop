using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface IEmployeeRepository
	{
		Task<Employee?> GetByIdAsync(Guid id);
		//Task<Employee?> GetByUsernameAsync(string username);
		Task<Employee?> GetByEmailAsync(string email);
		Task<IEnumerable<Employee>> GetAllAsync();
		Task<Employee> AddEmployeeAsync(Employee employee);
		Task<Employee> UpdateEmployeeAsync(Guid id, Employee employee);
		Task<bool> DeleteEmployeeAsync(Guid id);
		//Task<bool> UsernameExistsAsync(string username);
		Task<bool> EmailExistsAsync(string email);
	}
}
