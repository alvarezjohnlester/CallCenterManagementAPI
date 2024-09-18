using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;

namespace UnitTests
{
	[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
	public class CustomerRepositoryTests
	{
		private CallCenterManagementAPIContext _context;
		private CustomerRepository _repository;

		private void InitializeDatabase()
		{
			var options = new DbContextOptionsBuilder<CallCenterManagementAPIContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new CallCenterManagementAPIContext(options);
			_repository = new CustomerRepository(_context);
		}

		[Fact, Priority(1)]
		public async Task AddAsync_ShouldAddCustomer()
		{
			// Arrange
			InitializeDatabase();
			var customer = new Customer { Id = 1, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneNumber = "1234567890" };

			// Act
			await _repository.AddAsync(customer);

			// Assert
			var addedCustomer = await _context.Customer.FindAsync(customer.Id);
			Assert.NotNull(addedCustomer);
			Assert.Equal("Jane Doe", addedCustomer.Name);
		}

		[Fact, Priority(2)]
		public async Task DeleteAsync_ShouldMarkCustomerAsDeleted()
		{
			// Arrange
			InitializeDatabase();
			var customer = new Customer { Id = 1, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneNumber = "1234567890" };
			await _context.Customer.AddAsync(customer);
			await _context.SaveChangesAsync();

			// Act
			await _repository.DeleteAsync(customer.Id);

			// Assert
			var deletedCustomer = await _context.Customer.FindAsync(customer.Id);
			Assert.NotNull(deletedCustomer);
			Assert.True(deletedCustomer.IsDeleted);
		}

		[Fact, Priority(3)]
		public async Task GetAllAsync_ShouldReturnCustomers()
		{
			// Arrange
			InitializeDatabase();
			var customers = new List<Customer>
			{
				new Customer { Id = 1, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneNumber = "1234567890" },
				new Customer { Id = 2, Name = "John Smith", Email = "john.smith@example.com", PhoneNumber = "0987654321" }
			};
			await _context.Customer.AddRangeAsync(customers);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetAllAsync(1, 10);

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact, Priority(4)]
		public async Task GetByIdAsync_ShouldReturnCustomer()
		{
			// Arrange
			InitializeDatabase();
			var customer = new Customer { Id = 1, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneNumber = "1234567890" };
			await _context.Customer.AddAsync(customer);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetByIdAsync(customer.Id);

			// Assert
			Assert.Equal(customer, result);
		}

		[Fact, Priority(5)]
		public async Task UpdateAsync_ShouldUpdateCustomer()
		{
			// Arrange
			InitializeDatabase();
			var customer = new Customer { Id = 1, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneNumber = "1234567890" };
			await _context.Customer.AddAsync(customer);
			await _context.SaveChangesAsync();

			// Act
			customer.Name = "John Smith";
			await _repository.UpdateAsync(customer);

			// Assert
			var updatedCustomer = await _context.Customer.FindAsync(customer.Id);
			Assert.Equal("John Smith", updatedCustomer.Name);
		}
	}
}
