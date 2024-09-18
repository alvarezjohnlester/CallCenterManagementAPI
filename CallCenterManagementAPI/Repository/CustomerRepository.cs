using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CallCenterManagementAPI.Repository
{
	public class CustomerRepository : IRepository<Customer>
	{
		private readonly CallCenterManagementAPIContext _context;
		public CustomerRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}
		public async Task AddAsync(Customer entity)
		{
			await _context.Customer.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var customer = await _context.Customer.FindAsync(id);
			if (customer != null)
			{
				customer.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize)
		{
			return await _context.Customer.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<Customer> GetByIdAsync(int id)
		{
			return await _context.Customer.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task UpdateAsync(Customer entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
	}
}
