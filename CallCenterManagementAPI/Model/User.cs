using System.Text.Json.Serialization;

namespace CallCenterManagementAPI.Model
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		[JsonIgnore]
		public bool IsDeleted { get; set; }
	}
}
