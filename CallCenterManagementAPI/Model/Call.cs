using CallCenterManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CallCenterManagementAPI.Model
{
	public class Call
	{
		[Key]
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int? AgentId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public CallStatus Status { get; set; }
		public string Notes { get; set; }
		[JsonIgnore]
		public bool IsDeleted { get; set; }
	}
}
