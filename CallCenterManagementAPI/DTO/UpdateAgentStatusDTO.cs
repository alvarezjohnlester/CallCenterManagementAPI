using CallCenterManagementAPI.Enums;

namespace CallCenterManagementAPI.DTO
{
	public class UpdateAgentStatusDTO
	{
		public int Id { get; set; }
		public AgentStatus Status { get; set; }
	}
}
