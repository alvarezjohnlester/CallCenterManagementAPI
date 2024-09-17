using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Repository
{
	public class TicketRepository : IRepository<Ticket>
	{
		private readonly CallCenterManagementAPIContext _context;
		public TicketRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}

		public Task AddAsync(Ticket entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Ticket>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Ticket>> GetAllAsync(int pageNumber, int pageSize)
		{
			throw new NotImplementedException();
		}

		public Task<Ticket> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Ticket entity)
		{
			throw new NotImplementedException();
		}
	}
}
