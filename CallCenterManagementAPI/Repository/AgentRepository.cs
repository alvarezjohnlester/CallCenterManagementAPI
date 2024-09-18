using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CallCenterManagementAPI.Repository
{
	public class AgentRepository : IAgentRepository
	{
		private readonly CallCenterManagementAPIContext _context;
		public AgentRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}
		public async Task AddAsync(Agent entity)
		{
			await _context.Agent.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var agent = await _context.Agent.FindAsync(id);
			if (agent != null)
			{
				agent.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Agent>> GetAllAsync(int pageNumber, int pageSize)
		{
			return await _context.Agent.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<Agent> GetByIdAsync(int id)
		{
			return await _context.Agent.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task UpdateAsync(Agent entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<Agent>> GetAvailableAgentsAsync()
		{
			return await _context.Agent
				.Where(agent => agent.Status == Enums.AgentStatus.Available && !agent.IsDeleted)
				.ToListAsync();
		}
	}
}
