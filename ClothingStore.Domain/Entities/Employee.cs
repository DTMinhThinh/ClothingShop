using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("Employees")]
	public class Employee : User
	{
		public Decimal Salary { get; set; }
		public Guid? RoleID { get; set; }  // Khóa ngoại RoleID
										   // Constructor cho tạo mới (Create)
		public Employee(string password, string name, string? email,
					   DateTime dateOfBirth, decimal salary, Guid? roleId = null,
					   string? phone = null, string? address = null)
			: base(password, name, email, dateOfBirth, phone, address)
		{
			Salary = salary;
			RoleID = roleId;
		}

		// Constructor không tham số hỗ trợ deserialization
		public Employee() : base() { }


	}
}
