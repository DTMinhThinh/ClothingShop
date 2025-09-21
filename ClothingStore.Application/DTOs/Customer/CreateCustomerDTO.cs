using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Application.DTOs.Customer
{
	public class CreateCustomerDTO
	{
		[Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3-50 ký tự")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Mật khẩu là bắt buộc")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-100 ký tự")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Họ tên là bắt buộc")]
		[StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 ký tự")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email là bắt buộc")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }
		[DataType(DataType.Date)]
		[Range(typeof(DateTime), "1/1/1900", "1/1/2100", ErrorMessage = "Ngày sinh không hợp lệ")]
		public DateTime DateOfBirth { get; set; }
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		public string? Phone { get; set; }
		[StringLength(200, ErrorMessage = "Địa chỉ không vượt quá 200 ký tự")]
		public string? Address { get; set; }
		[StringLength(20, ErrorMessage = "Số tài khoản không vượt quá 20 ký tự")]
		public string? BankAccountNumber { get; set; }
	}
}
