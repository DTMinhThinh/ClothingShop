using ClothingStore.Application.Interfaces;
using ClothingStore.Domain.Entities;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly ClothingStoreDBContext _context;

		public EmployeeRepository(ClothingStoreDBContext context)
		{
			_context = context;
		}

		public async Task<Employee?> GetByIdAsync(Guid id)
		{
			var query = from e in _context.Employees
						where e.Id == id
						select e;
			return query.FirstOrDefault();
		}

		//public async Task<Employee?> GetByUsernameAsync(string username)
		//{
		//	return await _context.Employees
		//		.FirstOrDefaultAsync(e => e.Username == username);
		//}

		public async Task<Employee?> GetByEmailAsync(string email)
		{
			return await _context.Employees
				.FirstOrDefaultAsync(e => e.Email == email);
		}

		public async Task<IEnumerable<Employee>> GetAllAsync()
		{
			var query = from e in _context.Employees
							//join o in _context.Orders on e.Id equals o.EmployeeId
							//join r in _context.Roles on e.RoleID equals r.RoleId
						select e;
			//return await _context.Employees
			//	//.Include(e => e.Role)
			//	//.Include(e => e.ApprovedOrders)
			//	.ToListAsync();
			return await query.Distinct().ToListAsync();
		}

		public async Task<Employee> AddEmployeeAsync(Employee employee)
		{
			// Validate business rules
			await ValidateEmployeeBeforeAddAsync(employee);

			// Set các giá trị mặc định
			employee.DateCreated = DateTime.UtcNow;
			employee.IsActive = true;

			await _context.Employees.AddAsync(employee);
			await _context.SaveChangesAsync();
			return employee;
		}

		private async Task ValidateEmployeeBeforeAddAsync(Employee employee)
		{
			//if (await _context.Employees.AnyAsync(e => e.Username == employee.Username))
			//	throw new InvalidOperationException("Username already exists");

			if (await _context.Employees.AnyAsync(e => e.Email == employee.Email))
				throw new InvalidOperationException("Email already exists");

			//if (string.IsNullOrWhiteSpace(employee.Username))
			//	throw new ArgumentException("Username is required");

			if (string.IsNullOrWhiteSpace(employee.Password))
				throw new ArgumentException("Password is required");

			if (string.IsNullOrWhiteSpace(employee.Name))
				throw new ArgumentException("Name is required");
		}

		public async Task<Employee> UpdateEmployeeAsync(Guid id, Employee employee)
		{
			var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
			if (existingEmployee is null)
				throw new KeyNotFoundException($"Employee with ID {id} not found");

			// Cập nhật các trường cần thiết
			existingEmployee.Name = employee.Name ?? existingEmployee.Name;
			existingEmployee.Email = employee.Email ?? existingEmployee.Email;
			existingEmployee.Phone = employee.Phone ?? existingEmployee.Phone;
			if (employee.DateOfBirth != default(DateTime))
			{
				existingEmployee.DateOfBirth = employee.DateOfBirth;
			}
			existingEmployee.Address = employee.Address ?? existingEmployee.Address;
			existingEmployee.Salary = employee.Salary != 0 ? employee.Salary : existingEmployee.Salary; // Cập nhật lương

			// Chỉ update các field cần thiết, không update Username/Password ở đây
			await _context.SaveChangesAsync();
			return existingEmployee;
		}

		public async Task<bool> DeleteEmployeeAsync(Guid id)
		{
			var deleteEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
			if (deleteEmployee != null)
			{
				_context.Employees.Remove(deleteEmployee);
				return await _context.SaveChangesAsync() > 0;
			}
			return false;
		}

		//public async Task<bool> UsernameExistsAsync(string username)
		//{
		//	return await _context.Employees
		//		.AnyAsync(e => e.Username == username);
		//}

		public async Task<bool> EmailExistsAsync(string email)
		{
			return await _context.Employees
				.AnyAsync(e => e.Email == email);
		}
	}
}
