using CallCenterManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CallCenterManagementAPI.Model
{
	public class Ticket
	{
		[Key]
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int? AgentId { get; set; }
		public TicketStatus Status { get; set; }
		public TicketPriority Priority { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string Description { get; set; }
		public string Resolution { get; set; }
		[JsonIgnore]
		public bool IsDeleted { get; set; }
	}
}
