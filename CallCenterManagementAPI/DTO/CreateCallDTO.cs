using CallCenterManagementAPI.Enums;

namespace CallCenterManagementAPI.DTO
{
	public class CreateCallDTO
	{
		public int CustomerId { get; set; }
		public int? AgentId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public string Notes { get; set; }
	}
}
