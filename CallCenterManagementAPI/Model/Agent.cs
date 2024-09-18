using CallCenterManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CallCenterManagementAPI.Model
{
	public class Agent
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneExtension { get; set; }
		public AgentStatus Status { get; set; }
		[JsonIgnore]
		public bool IsDeleted { get; set; }
	}
}
