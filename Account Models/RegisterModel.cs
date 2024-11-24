using System.ComponentModel.DataAnnotations;

namespace Web_Api.Account_Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = ".ایمیل خود را وارد کنید")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = ".پسورد خود را وارد کنید")]
		[RegularExpression(@"(^[A-Z][a-zA-Z0-9]{4,19}$)", ErrorMessage = "رمز عبور شما باید حداقل 5 و حداکثر 20 کاراکتر باشد و حرف اول آن یک حرف بزرگ انگلیسی باشد و حتماً شامل عدد و حروف باشد.")]
		public string Password { get; set; }

		[Required(ErrorMessage = ".پسورد خود را مجددا وارد کنید")]
		[Compare("Password", ErrorMessage = "!پسورد ها یکسان نیست")]
		public string ConfirmPassword { get; set; }
	}
}
