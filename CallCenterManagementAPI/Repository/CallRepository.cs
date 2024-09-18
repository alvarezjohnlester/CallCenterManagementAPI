using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CallCenterManagementAPI.Repository
{
	public class CallRepository : IRepository<Call>
	{
		private readonly CallCenterManagementAPIContext _context;
		public CallRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Call entity)
		{
			await _context.Call.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var call = await _context.Call.FindAsync(id);
			if (call != null)
			{
				call.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Call>> GetAllAsync(int pageNumber, int pageSize)
		{
			return await _context.Call.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<Call> GetByIdAsync(int id)
		{
			return await _context.Call.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task UpdateAsync(Call entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
	}
}
