using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.Repository;

namespace CallCenterManagementAPI.Service
{
	public class CallRoutingService : ICallRoutingService
	{
		private readonly IAgentRepository _agentRepository;
		private int _lastAssignedAgentIndex = -1;
		private readonly IRepository<Call> _callRepository;

		public CallRoutingService(IAgentRepository agentRepository, IRepository<Call> callRepository)
		{
			_agentRepository = agentRepository;
			_callRepository = callRepository;
		}

		public async Task<Agent> AssignCallToAgentAsync(int callId)
		{
			var call = await _callRepository.GetByIdAsync(callId);
			if (call == null)
			{
				return null; // Call not found
			}

			var agents = await _agentRepository.GetAvailableAgentsAsync();
			if (!agents.Any())
			{
				return null; // No agents available
			}

			_lastAssignedAgentIndex = (_lastAssignedAgentIndex + 1) % agents.Count();
			var assignedAgent = agents.ElementAt(_lastAssignedAgentIndex);

			call.AgentId = assignedAgent.Id;
			call.Status = Enums.CallStatus.InProgress;
			await _callRepository.UpdateAsync(call);

			return assignedAgent;
		}
	}
}

