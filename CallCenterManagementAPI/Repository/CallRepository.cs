using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Repository
{
	public class CallRepository : IRepository<Call>
	{
		private readonly CallCenterManagementAPIContext _context;
		public CallRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}

		public Task AddAsync(Call entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Call>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Call>> GetAllAsync(int pageNumber, int pageSize)
		{
			throw new NotImplementedException();
		}

		public Task<Call> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Call entity)
		{
			throw new NotImplementedException();
		}
	}
}
