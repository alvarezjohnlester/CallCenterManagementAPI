namespace CallCenterManagementAPI.Interface
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
		Task<T> GetByIdAsync(int id);
		Task AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(int id);
	}
}
