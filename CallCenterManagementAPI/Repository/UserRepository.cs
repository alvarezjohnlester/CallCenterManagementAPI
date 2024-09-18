using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CallCenterManagementAPI.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly CallCenterManagementAPIContext _context;

		public UserRepository(CallCenterManagementAPIContext context)
		{
			_context = context;
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task AddUserAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}
	}
}
