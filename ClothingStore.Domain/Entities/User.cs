using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Domain.Entities
{
	public abstract class User
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[EmailAddress]
		public string? Email { get; set; }
		public bool IsActive { get; set; } = true;

		//public string Username { get; private set; } 

		public string Password { get; private set; }

		public DateTime DateCreated { get; set; } = DateTime.UtcNow;

		public string Name { get; set; }

		public string? Phone { get; set; }

		public DateTime DateOfBirth { get; set; }

		public string? Address { get; set; }

		public void Active()
		{
			IsActive = true;
		}

		public void DeActive()
		{
			IsActive = false;
		}

		// Constructor không tham số hỗ trợ deserialization
		protected User() { }

		// Constructor cho yêu cầu với tham số
		protected User(string password, string name, string? email,
					  DateTime dateOfBirth, string? phone = null, string? address = null)
		{

			//Username = username ?? throw new ArgumentNullException(nameof(username));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Email = email;
			DateOfBirth = dateOfBirth;
			Phone = phone;
			Address = address;

			DateCreated = DateTime.UtcNow;
			IsActive = true;
		}

		public void ResetPassword(string newPassword)
		{
			Password = newPassword;
		}
		//public void ChangeUsername(string newUsername)
		//{
		//	Username = newUsername;
		//}

	}
}
