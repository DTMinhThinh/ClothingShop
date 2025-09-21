namespace ClothingStore.Application.DTOs.Employee
{
	public class CreateEmployeeDTO
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public decimal Salary { get; set; }
		public Guid? RoleID { get; set; }
	}
}
