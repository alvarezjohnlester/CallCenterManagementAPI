using CallCenterManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CallCenterManagementAPI.Model
{
	public class Customer
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? LastContactDate { get; set; } 
		[JsonIgnore]
		public bool IsDeleted { get; set; }
	}
}
