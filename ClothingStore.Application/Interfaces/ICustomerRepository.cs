using ClothingStore.Application.DTOs.Customer;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application.Interfaces
{
	public interface ICustomerRepository
	{
		Task<Customer?> GetByIdAsync(Guid id);
		//Task<Customer?> GetByUsernameAsync(string username);
		Task<Customer?> GetByEmailAsync(string email);
		Task<IEnumerable<Customer>> GetAllAsync();
		Task<Customer> AddCustomerAsync(Customer customer);
		Task<Customer> UpdateCustomerAsync(Guid id, UpdateProfileCustomerDTO dto);
		Task<bool> DeleteCustomerAsync(Guid id);
		//Task<bool> UsernameExistsAsync(string username);
		Task<bool> EmailExistsAsync(string email);
	}
}
