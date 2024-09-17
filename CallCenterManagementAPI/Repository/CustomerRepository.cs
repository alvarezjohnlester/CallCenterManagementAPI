using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Repository
{
	public class CustomerRepository : IRepository<Customer>
	{
		private readonly CallCenterManagementAPIContext _context;
		public CustomerRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}
		public Task AddAsync(Customer entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Customer>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Customer entity)
		{
			throw new NotImplementedException();
		}
	}
}
