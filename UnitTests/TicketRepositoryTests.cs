using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Enums;
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
	public class TicketRepositoryTests
	{
		private CallCenterManagementAPIContext _context;
		private TicketRepository _repository;

		private void InitializeDatabase()
		{
			var options = new DbContextOptionsBuilder<CallCenterManagementAPIContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new CallCenterManagementAPIContext(options);
			_repository = new TicketRepository(_context);
		}

		[Fact, Priority(1)]
		public async Task AddAsync_ShouldAddTicket()
		{
			// Arrange
			InitializeDatabase();
			var ticket = new Ticket { Id = 1, CustomerId = 1, AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, Description = "Test ticket", Resolution = "Pending" };

			// Act
			await _repository.AddAsync(ticket);

			// Assert
			var addedTicket = await _context.Ticket.FindAsync(ticket.Id);
			Assert.NotNull(addedTicket);
			Assert.Equal("Test ticket", addedTicket.Description);
		}

		[Fact, Priority(2)]
		public async Task DeleteAsync_ShouldMarkTicketAsDeleted()
		{
			// Arrange
			InitializeDatabase();
			var ticket = new Ticket { Id = 1, CustomerId = 1, AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, Description = "Test ticket", Resolution = "Pending" };
			await _context.Ticket.AddAsync(ticket);
			await _context.SaveChangesAsync();

			// Act
			await _repository.DeleteAsync(ticket.Id);

			// Assert
			var deletedTicket = await _context.Ticket.FindAsync(ticket.Id);
			Assert.NotNull(deletedTicket);
			Assert.True(deletedTicket.IsDeleted);
		}

		[Fact, Priority(3)]
		public async Task GetAllAsync_ShouldReturnTickets()
		{
			// Arrange
			InitializeDatabase();
			var tickets = new List<Ticket>
			{
				new Ticket { Id = 1, CustomerId = 1, AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, Description = "Test ticket 1", Resolution = "Pending" },
				new Ticket { Id = 2, CustomerId = 2, AgentId = 2, Status = TicketStatus.Closed, Priority = TicketPriority.Low, CreatedAt = DateTime.Now, Description = "Test ticket 2", Resolution = "Resolved" }
			};
			await _context.Ticket.AddRangeAsync(tickets);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetAllAsync(1, 10);

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact, Priority(4)]
		public async Task GetByIdAsync_ShouldReturnTicket()
		{
			// Arrange
			InitializeDatabase();
			var ticket = new Ticket { Id = 1, CustomerId = 1, AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, Description = "Test ticket", Resolution = "Pending" };
			await _context.Ticket.AddAsync(ticket);
			await _context.SaveChangesAsync();

			// Act
			var result = await _repository.GetByIdAsync(ticket.Id);

			// Assert
			Assert.Equal(ticket, result);
		}

		[Fact, Priority(5)]
		public async Task UpdateAsync_ShouldUpdateTicket()
		{
			// Arrange
			InitializeDatabase();
			var ticket = new Ticket { Id = 1, CustomerId = 1, AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, Description = "Test ticket", Resolution = "Pending" };
			await _context.Ticket.AddAsync(ticket);
			await _context.SaveChangesAsync();

			// Act
			ticket.Description = "Updated test ticket";
			ticket.Resolution = "Resolved";
			await _repository.UpdateAsync(ticket);

			// Assert
			var updatedTicket = await _context.Ticket.FindAsync(ticket.Id);
			Assert.Equal("Updated test ticket", updatedTicket.Description);
			Assert.Equal("Resolved", updatedTicket.Resolution);
		}
	}
}
