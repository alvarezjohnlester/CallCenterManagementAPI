using CallCenterManagementAPI.Model;
using NuGet.Protocol.Core.Types;

namespace CallCenterManagementAPI.Interface
{
	public interface IAgentRepository : IRepository<Agent>
	{
		Task<IEnumerable<Agent>> GetAvailableAgentsAsync();
	}
}
