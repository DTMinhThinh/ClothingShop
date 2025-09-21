using ClothingStore.Application.DTOs.Customer;
using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly ClothingStoreDBContext _context;

		public CustomerRepository(ClothingStoreDBContext context)
		{
			_context = context;
		}

		public async Task<Customer?> GetByIdAsync(Guid id)
		{
			return await _context.Customers
				//.Include(c => c.Orders)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		//public async Task<Customer?> GetByUsernameAsync(string username)
		//{
		//	return await _context.Customers
		//		.FirstOrDefaultAsync(c => c.Username == username);
		//}

		public async Task<Customer?> GetByEmailAsync(string email)
		{
			return await _context.Customers
				.FirstOrDefaultAsync(c => c.Email == email);
		}

		public async Task<IEnumerable<Customer>> GetAllAsync()
		{
			return await _context.Customers
				//.Include(c => c.Orders)
				.ToListAsync();
		}

		public async Task<Customer> AddCustomerAsync(Customer customer)
		{
			// Validate business rules
			await ValidateCustomerBeforeAddAsync(customer);

			// Set các giá trị mặc định
			customer.DateCreated = DateTime.UtcNow;
			customer.IsActive = true;

			await _context.Customers.AddAsync(customer);
			await _context.SaveChangesAsync();
			return customer;
		}

		private async Task ValidateCustomerBeforeAddAsync(Customer customer)
		{
			//if (await _context.Customers.AnyAsync(c => c.Username == customer.Username))
			//	throw new InvalidOperationException("Username already exists");

			if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
				throw new InvalidOperationException("Email already exists");

			//if (string.IsNullOrWhiteSpace(customer.Username))
			//	throw new ArgumentException("Username is required");

			if (string.IsNullOrWhiteSpace(customer.Password))
				throw new ArgumentException("Password is required");

			if (string.IsNullOrWhiteSpace(customer.Name))
				throw new ArgumentException("Name is required");
		}

		public async Task<Customer> UpdateCustomerAsync(Guid id, UpdateProfileCustomerDTO dto)
		{
			var customer = await _context.Customers.FindAsync(id);
			if (customer == null)
				throw new KeyNotFoundException($"Customer with ID {id} not found");

			// Validate business rules
			if (!string.IsNullOrEmpty(dto.Email) &&
				await _context.Customers.AnyAsync(c => c.Email == dto.Email && c.Id != id))
				throw new InvalidOperationException("Email already exists");

			// Update only allowed fields
			customer.Name = dto.Name;
			customer.Email = dto.Email;
			customer.DateOfBirth = dto.DateOfBirth;
			customer.Phone = dto.Phone;
			customer.Address = dto.Address;
			customer.BankAccountNumber = dto.BankAccountNumber;

			await _context.SaveChangesAsync();
			return customer;
		}

		public async Task<bool> DeleteCustomerAsync(Guid id)
		{
			var Deletecustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
			if (Deletecustomer != null)
			{
				_context.Customers.Remove(Deletecustomer);
				return await _context.SaveChangesAsync() > 0;
			}
			return false;
		}

		//public async Task<bool> UsernameExistsAsync(string username)
		//{
		//	return await _context.Customers
		//		.AnyAsync(c => c.Username == username);
		//}

		public async Task<bool> EmailExistsAsync(string email)
		{
			return await _context.Customers
				.AnyAsync(c => c.Email == email);
		}
	}
}
