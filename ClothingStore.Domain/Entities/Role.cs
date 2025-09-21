using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("Roles")]
	public class Role
	{
		public Guid RoleId { get; set; }
		public required string RoleName { get; set; }
		//public ICollection<Employee> Employees { get; set; } = [];
	}
}
