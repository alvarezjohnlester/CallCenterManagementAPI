using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Interface
{
	public interface IUserRepository
	{
		Task<User> GetUserByUsernameAsync(string username);
		Task AddUserAsync(User user);
	}
}
