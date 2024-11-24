using System.ComponentModel.DataAnnotations;

namespace Web_Api.DbModels
{
	public class PhoneBookDTO
	{
		[RegularExpression("^[آ-یa-zA-Z]{2,15}$", ErrorMessage = "!اسم شما باید حداقل 2 کاراکتر و حداکثر 15 کاراکتر باشد")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "!فامیلی نمیتواند خالی باشد")]
		[RegularExpression("^[آ-یa-zA-Z]{2,20}$", ErrorMessage = "!فامیلی شما باید حداقل 2 کاراکتر و حداکثر 20 کاراکتر باشد")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "!شماره تلفن نمیتواند خالی باشد")]
		[RegularExpression(@"^09\d{9}$", ErrorMessage = " !شماره باید 11 رقم و با '09' شروع شود ")]
		public string PhoneNumber { get; set; }
	}
}
