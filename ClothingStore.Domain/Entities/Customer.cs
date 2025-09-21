using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Domain.Entities
{
	[Table("Customers")]
	public class Customer : User
	{ // Create
		public Customer(string password, string name, string? email,
					   DateTime dateOfBirth, string? phone = null, string? address = null,
					   string? bankAccountNumber = null)
			: base(password, name, email, dateOfBirth, phone, address)
		{
			BankAccountNumber = bankAccountNumber;
		}
		public Customer() : base() { }

		public string? BankAccountNumber { get; set; }

	}
}
