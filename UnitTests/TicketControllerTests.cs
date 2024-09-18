using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CallCenterManagementAPI.Controllers;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Priority;

namespace UnitTests
{
	public class TicketsControllerTests
	{
		private readonly Mock<IRepository<Ticket>> _mockRepo;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<ILogger<TicketsController>> _mockLogger;
		private readonly TicketsController _controller;

		public TicketsControllerTests()
		{
			_mockRepo = new Mock<IRepository<Ticket>>();
			_mockMapper = new Mock<IMapper>();
			_mockLogger = new Mock<ILogger<TicketsController>>();
			_controller = new TicketsController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
		}

		[Fact, Priority(1)]
		public async Task GetTicketListAsync_ReturnsOkResult()
		{
			// Arrange
			var tickets = new List<Ticket> { new Ticket { Id = 1, Status = CallCenterManagementAPI.Enums.TicketStatus.Open } };
			_mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(tickets);

			// Act
			var result = await _controller.GetTicketListAsync();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<List<Ticket>>(okResult.Value);
			Assert.Single(returnValue);
		}

		[Fact, Priority(2)]
		public async Task GetTicketAsync_ReturnsNotFoundResult_WhenTicketNotFound()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket)null);

			// Act
			var result = await _controller.GetTicketAsync(1);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact, Priority(3)]
		public async Task GetTicketAsync_ReturnsOkResult_WhenTicketFound()
		{
			// Arrange
			var ticket = new Ticket { Id = 1, Status = CallCenterManagementAPI.Enums.TicketStatus.Open };
			_mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);

			// Act
			var result = await _controller.GetTicketAsync(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<Ticket>(okResult.Value);
			Assert.Equal(1, returnValue.Id);
		}

		[Fact, Priority(4)]
		public async Task CreateTicketAsync_ReturnsCreatedAtActionResult()
		{
			// Arrange
			var ticketDto = new CreateTicketDTO { /* Initialize properties */ };
			var ticket = new Ticket { Id = 1, Status = CallCenterManagementAPI.Enums.TicketStatus.Open };
			_mockMapper.Setup(m => m.Map<Ticket>(It.IsAny<CreateTicketDTO>())).Returns(ticket);
			_mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.CreateTicketAsync(ticketDto);

			// Assert
			var createdAtActionResult = Assert.IsType<CreatedResult>(result.Result);
			var returnValue = Assert.IsType<Ticket>(createdAtActionResult.Value);
			Assert.Equal(1, returnValue.Id);
		}

		[Fact, Priority(5)]
		public async Task UpdateTicketAsync_ReturnsOkResult()
		{
			// Arrange
			var ticketDto = new UpdateTicketDTO { Id = 1, /* Initialize other properties */ };
			var ticket = new Ticket { Id = 1, Status = CallCenterManagementAPI.Enums.TicketStatus.Open };
			_mockMapper.Setup(m => m.Map<Ticket>(It.IsAny<UpdateTicketDTO>())).Returns(ticket);
			_mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.UpdateTicketAsync(ticketDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Successfully Updated", okResult.Value);
		}

		[Fact, Priority(6)]
		public async Task DeleteTicketAsync_ReturnsOkResult()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.DeleteTicketAsync(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Sucessfully Deleted", okResult.Value);
		}
	}
}