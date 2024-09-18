using AutoMapper;
using CallCenterManagementAPI.Controllers;
using CallCenterManagementAPI.DTO;
using CallCenterManagementAPI.Enums;
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
	public class AgentsControllerTests
	{
		private readonly Mock<IRepository<Agent>> _mockRepo;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<ILogger<AgentsController>> _mockLogger;
		private readonly AgentsController _controller;

		public AgentsControllerTests()
		{
			_mockRepo = new Mock<IRepository<Agent>>();
			_mockMapper = new Mock<IMapper>();
			_mockLogger = new Mock<ILogger<AgentsController>>();
			_controller = new AgentsController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
		}

		[Fact, Priority(1)]
		public async Task GetAgentListAsync_ShouldReturnAgents()
		{
			// Arrange
			var agents = new List<Agent>
		{
			new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available },
			new Agent { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com", PhoneExtension = "5678", Status = AgentStatus.Available }
		};
			_mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(agents);

			// Act
			var result = await _controller.GetAgentListAsync();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnAgents = Assert.IsType<List<Agent>>(okResult.Value);
			Assert.Equal(2, returnAgents.Count);
		}

		[Fact, Priority(2)]
		public async Task GetAgentAsync_ShouldReturnAgent()
		{
			// Arrange
			var agent = new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			_mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(agent);

			// Act
			var result = await _controller.GetAgentAsync(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnAgent = Assert.IsType<Agent>(okResult.Value);
			Assert.Equal(agent, returnAgent);
		}

		[Fact, Priority(3)]
		public async Task CreateAgent_ShouldAddAgent()
		{
			// Arrange
			var agentDto = new CreateAgentDTO { Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234" };
			var agent = new Agent { Id = 1, Name = "John Doe", Email = "john.doe@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			_mockMapper.Setup(mapper => mapper.Map<Agent>(agentDto)).Returns(agent);
			_mockRepo.Setup(repo => repo.AddAsync(agent)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.CreateAgent(agentDto);

			// Assert
			var createdResult = Assert.IsType<CreatedResult>(result.Result);
			var returnAgent = Assert.IsType<Agent>(createdResult.Value);
			Assert.Equal(agent, returnAgent);
		}

		[Fact, Priority(4)]
		public async Task UpdateAgent_ShouldUpdateAgent()
		{
			// Arrange
			var agentDto = new UpdateAgentDTO { Id = 1, Name = "John Smith", Email = "john.smith@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			var agent = new Agent { Id = 1, Name = "John Smith", Email = "john.smith@example.com", PhoneExtension = "1234", Status = AgentStatus.Available };
			_mockMapper.Setup(mapper => mapper.Map<Agent>(agentDto)).Returns(agent);
			_mockRepo.Setup(repo => repo.UpdateAsync(agent)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.UpdateAgent(agentDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Successfully Updated", okResult.Value);
		}

		[Fact, Priority(5)]
		public async Task DeleteAgent_ShouldMarkAgentAsDeleted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.DeleteAgent(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Sucessfully Deleted", okResult.Value);
		}
	}
}
