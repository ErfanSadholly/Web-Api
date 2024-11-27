using FluentValidation;
using Web_Api.DbModels;

namespace Web_Api.Validations
{
	public class PhoneBookDTOValidation : AbstractValidator<PhoneBookDTO>
	{
		public PhoneBookDTOValidation()
		{
			RuleFor(x => x.FirstName.Trim())
				.Must(firstname => firstname.All(char.IsLetter)).WithMessage("!اسم باید فقط شامل حروف باشد")
				.MinimumLength(3).WithMessage("!اسم شما باید حداقل 3 کاراکتر باشد")
				.MaximumLength(15).WithMessage("!اسم نمیتواند بیشتر از 15 کاراکتر باشد");

			//=============================================================================================================

			RuleFor(x => x.LastName.Trim()).NotEmpty().WithMessage("!فامیلی نمیتواند خالی باشد")
				.Must(lastname => lastname.All(char.IsLetter)).WithMessage("!فامیلی باید فقط شامل حروف باشد")
				.MinimumLength(3).WithMessage("!فامیلی شما باید حداقل 3 کاراکتر باشد")
				.MaximumLength(20).WithMessage("!فامیلی نمیتواند بیشتر از 20 کاراکتر باشد");

			//=============================================================================================================

			RuleFor(x => x.PhoneNumber.Trim()).NotEmpty().WithMessage("!شماره تلفن نمیتواند خالی باشد")
				.Length(11).WithMessage("!شماره تلفن باید 11 رقم باشد")
				.Must(phonenumber => phonenumber.All(char.IsDigit)).WithMessage("!شماره تلفن باید فقط شامل اعداد باشد")
				.Must(phonenumber => phonenumber.StartsWith("09")).WithMessage("شماره تلفن باید با 0و9 شروع شود");
		}
	}
}
