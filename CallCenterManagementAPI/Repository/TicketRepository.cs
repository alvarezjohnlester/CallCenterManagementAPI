using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CallCenterManagementAPI.Repository
{
	public class TicketRepository : IRepository<Ticket>
	{
		private readonly CallCenterManagementAPIContext _context;
		public TicketRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Ticket entity)
		{
			await _context.Ticket.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var customer = await _context.Ticket.FindAsync(id);
			if (customer != null)
			{
				customer.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
		}
		public async Task<IEnumerable<Ticket>> GetAllAsync(int pageNumber, int pageSize)
		{
			return await _context.Ticket.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<Ticket> GetByIdAsync(int id)
		{
			return await _context.Ticket.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task UpdateAsync(Ticket entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
	}
}
