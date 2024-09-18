using AutoMapper;
using CallCenterManagementAPI.Controllers;
using CallCenterManagementAPI.DTO;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;

namespace UnitTests
{
	[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
	public class CallsControllerTests
	{
		private readonly Mock<IRepository<Call>> _mockRepo;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<ILogger<CallsController>> _mockLogger;
		private readonly CallsController _controller;

		public CallsControllerTests()
		{
			_mockRepo = new Mock<IRepository<Call>>();
			_mockMapper = new Mock<IMapper>();
			_mockLogger = new Mock<ILogger<CallsController>>();
			_controller = new CallsController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
		}

		[Fact, Priority(1)]
		public async Task GetCallListAsync_ReturnsOkResult()
		{
			// Arrange
			var calls = new List<Call> { new Call { Id = 1, Status = CallCenterManagementAPI.Enums.CallStatus.Queued } };
			_mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(calls);

			// Act
			var result = await _controller.GetCallListAsync();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<List<Call>>(okResult.Value);
			Assert.Single(returnValue);
		}

		[Fact, Priority(2)]
		public async Task GetCallAsync_ReturnsNotFoundResult_WhenCallNotFound()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Call)null);

			// Act
			var result = await _controller.GetCallAsync(1);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact, Priority(3)]
		public async Task GetCallAsync_ReturnsOkResult_WhenCallFound()
		{
			// Arrange
			var call = new Call { Id = 1, Status = CallCenterManagementAPI.Enums.CallStatus.Queued };
			_mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(call);

			// Act
			var result = await _controller.GetCallAsync(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<Call>(okResult.Value);
			Assert.Equal(1, returnValue.Id);
		}

		[Fact, Priority(4)]
		public async Task CreateCall_ReturnsCreatedAtActionResult()
		{
			// Arrange
			var callDto = new CreateCallDTO { /* Initialize properties */ };
			var call = new Call { Id = 1, Status = CallCenterManagementAPI.Enums.CallStatus.Queued };
			_mockMapper.Setup(m => m.Map<Call>(It.IsAny<CreateCallDTO>())).Returns(call);
			_mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Call>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.CreateCall(callDto);

			// Assert
			var createdAtActionResult = Assert.IsType<CreatedResult>(result.Result);
			var returnValue = Assert.IsType<Call>(createdAtActionResult.Value);
			Assert.Equal(1, returnValue.Id);
		}

		[Fact, Priority(5)]
		public async Task UpdateCallAsync_ReturnsOkResult()
		{
			// Arrange
			var callDto = new UpdateCallDTO { Id = 1, /* Initialize other properties */ };
			var call = new Call { Id = 1, Status = CallCenterManagementAPI.Enums.CallStatus.Queued };
			_mockMapper.Setup(m => m.Map<Call>(It.IsAny<UpdateCallDTO>())).Returns(call);
			_mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Call>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.UpdateCallAsync(callDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Successfully Updated", okResult.Value);
		}

		[Fact, Priority(6)]
		public async Task DeleteCall_ReturnsOkResult()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.DeleteCall(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Sucessfully Deleted", okResult.Value);
		}
	}
}
