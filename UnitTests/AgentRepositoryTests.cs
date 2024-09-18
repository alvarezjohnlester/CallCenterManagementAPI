

using Xunit;

using Moq;
using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Repository;
using CallCenterManagementAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.InMemory;
using CallCenterManagementAPI.Enums;
using System;
using Xunit.Priority;
namespace UnitTests
{
	[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
	public class AgentRepositoryTests
	{
		private CallCenterManagementAPIContext _context;
		private AgentRepository _repository;

		private void InitializeDatabase()
		{
			var options = new DbContextOptionsBuilder<CallCenterManagementAPIContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new CallCenterManagementAPIContext(options);
			_repository = new AgentRepository(_context);
		}

		[Fact, Priority(1)]
		public async Task AddAsync_ShouldAddAgent()
		{
			// Arrange
			InitializeDatabase();
			var agent = new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };

			// Act
			await _context.Agent.AddAsync(agent);
			await _context.SaveChangesAsync();

			// Assert
			var addedAgent = await _context.Agent.FindAsync(agent.Id);
			Assert.NotNull(addedAgent);
			Assert.Equal("John Doe", addedAgent.Name);
		}

		[Fact, Priority(2)]
		public async Task UpdateAsync_ShouldUpdateAgent()
		{
			// Arrange
			InitializeDatabase();
			var agent = new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			await _context.Agent.AddAsync(agent);
			await _context.SaveChangesAsync();

			// Act
			agent.Name = "John Smith";
			await _repository.UpdateAsync(agent);

			// Assert
			var updatedAgent = await _context.Agent.FindAsync(agent.Id);
			Assert.Equal("John Smith", updatedAgent.Name);
		}

		[Fact, Priority(3)]
		public async Task GetByIdAsync_ShouldReturnAgent()
		{
			// Arrange
			InitializeDatabase();
			var agent = new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			await _context.Agent.AddAsync(agent);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetByIdAsync(agent.Id);

			// Assert
			Assert.Equal(agent, result);
		}

		[Fact, Priority(4)]
		public async Task GetAllAsync_ShouldReturnAgents()
		{
			// Arrange
			InitializeDatabase();
			var agents = new List<Agent>
		{
			new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available },
			new Agent { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneExtension = "5678", Status = AgentStatus.Available }
		};

			await _context.Agent.AddRangeAsync(agents);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetAllAsync(1, 10);

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact, Priority(5)]
		public async Task DeleteAsync_ShouldMarkAgentAsDeleted()
		{
			// Arrange
			InitializeDatabase();
			var agent = new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			await _context.Agent.AddAsync(agent);
			await _context.SaveChangesAsync();

			// Act
			await _repository.DeleteAsync(agent.Id);

			// Assert
			var deletedAgent = await _context.Agent.FindAsync(agent.Id);
			Assert.True(deletedAgent.IsDeleted);
		}
	}
}
