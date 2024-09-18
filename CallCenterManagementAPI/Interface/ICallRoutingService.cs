using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Interface
{
	public interface ICallRoutingService
	{
		Task<Agent> AssignCallToAgentAsync(int callId);
	}
}
