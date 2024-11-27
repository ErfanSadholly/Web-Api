using System.ComponentModel.DataAnnotations;

namespace Web_Api.DbModels
{
	public class PhoneBookDTO
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
	}
}
