using CallCenterManagementAPI.Enums;

namespace CallCenterManagementAPI.DTO
{
	public class UpdateTicketDTO
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int? AgentId { get; set; }
		public TicketStatus Status { get; set; }
		public TicketPriority Priority { get; set; }
		public string Description { get; set; }
		public string Resolution { get; set; }
	}
}
