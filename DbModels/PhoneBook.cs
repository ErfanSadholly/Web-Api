using System.ComponentModel.DataAnnotations;

namespace Web_Api.DbModels
{
	public class PhoneBook
	{
		public int ID { get; set; }
		public string? FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public bool Deleted { get; set; } = false;
	}
}
