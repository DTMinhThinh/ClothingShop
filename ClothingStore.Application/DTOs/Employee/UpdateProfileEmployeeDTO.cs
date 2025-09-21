namespace ClothingStore.Application.DTOs.Employee
{
	public class UpdateProfileEmployeeDTO
	{
		public string Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public decimal Salary { get; set; }
		public DateTime DateOfBirth { get; set; }
		public Guid? RoleID { get; set; }
	}
}
