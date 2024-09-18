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
	public class CallRepositoryTests
	{
		private CallCenterManagementAPIContext _context;
		private CallRepository _repository;

		private void InitializeDatabase()
		{
			var options = new DbContextOptionsBuilder<CallCenterManagementAPIContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new CallCenterManagementAPIContext(options);
			_repository = new CallRepository(_context);
		}

		[Fact, Priority(1)]
		public async Task AddAsync_ShouldAddCall()
		{
			// Arrange
			InitializeDatabase();
			var call = new Call { Id = 1, CustomerId = 1, AgentId = 1, StartTime = DateTime.Now, Notes = "Test call" };

			// Act
			await _repository.AddAsync(call);

			// Assert
			var addedCall = await _context.Call.FindAsync(call.Id);
			Assert.NotNull(addedCall);
			Assert.Equal("Test call", addedCall.Notes);
		}

		[Fact, Priority(2)]
		public async Task DeleteAsync_ShouldMarkCallAsDeleted()
		{
			// Arrange
			InitializeDatabase();
			var call = new Call { Id = 1, CustomerId = 1, AgentId = 1, StartTime = DateTime.Now, Notes = "Test call" };
			await _context.Call.AddAsync(call);
			await _context.SaveChangesAsync();

			// Act
			await _repository.DeleteAsync(call.Id);

			// Assert
			var deletedCall = await _context.Call.FindAsync(call.Id);
			Assert.NotNull(deletedCall);
			Assert.True(deletedCall.IsDeleted);
		}

		[Fact, Priority(3)]
		public async Task GetAllAsync_ShouldReturnCalls()
		{
			// Arrange
			InitializeDatabase();
			var calls = new List<Call>
			{
				new Call { Id = 1, CustomerId = 1, AgentId = 1, StartTime = DateTime.Now, Notes = "Test call 1" },
				new Call { Id = 2, CustomerId = 2, AgentId = 2, StartTime = DateTime.Now, Notes = "Test call 2" }
			};
			await _context.Call.AddRangeAsync(calls);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetAllAsync(1, 10);

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact, Priority(4)]
		public async Task GetByIdAsync_ShouldReturnCall()
		{
			// Arrange
			InitializeDatabase();
			var call = new Call { Id = 1, CustomerId = 1, AgentId = 1, StartTime = DateTime.Now, Notes = "Test call" };
			await _context.Call.AddAsync(call);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetByIdAsync(call.Id);

			// Assert
			Assert.Equal(call, result);
		}

		[Fact, Priority(5)]
		public async Task UpdateAsync_ShouldUpdateCall()
		{
			// Arrange
			InitializeDatabase();
			var call = new Call { Id = 1, CustomerId = 1, AgentId = 1, StartTime = DateTime.Now, Notes = "Test call" };
			await _context.Call.AddAsync(call);
			await _context.SaveChangesAsync();

			// Act
			call.Notes = "Updated test call";
			await _repository.UpdateAsync(call);

			// Assert
			var updatedCall = await _context.Call.FindAsync(call.Id);
			Assert.Equal("Updated test call", updatedCall.Notes);
		}
	}
}
