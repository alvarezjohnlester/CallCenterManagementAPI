using CallCenterManagementAPI.Enums;

namespace CallCenterManagementAPI.DTO
{
	public class UpdateCallDTO
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int? AgentId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public CallStatus Status { get; set; }
		public string Notes { get; set; }
	}
}
