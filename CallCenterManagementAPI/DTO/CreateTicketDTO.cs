using CallCenterManagementAPI.Enums;

namespace CallCenterManagementAPI.DTO
{
	public class CreateTicketDTO
	{
		public int CustomerId { get; set; }
		public int? AgentId { get; set; } = 0;
		public TicketStatus Status { get; set; } = TicketStatus.Open;
		public TicketPriority Priority { get; set; } = TicketPriority.Low;
		public string Description { get; set; }
		public string Resolution { get; set; }
	}
}
