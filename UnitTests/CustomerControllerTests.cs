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
	public class CustomersControllerTests
	{
		private readonly Mock<IRepository<Customer>> _mockRepo;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<ILogger<CustomersController>> _mockLogger;
		private readonly CustomersController _controller;

		public CustomersControllerTests()
		{
			_mockRepo = new Mock<IRepository<Customer>>();
			_mockMapper = new Mock<IMapper>();
			_mockLogger = new Mock<ILogger<CustomersController>>();
			_controller = new CustomersController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
		}

		[Fact, Priority(1)]
		public async Task GetCustomerListAsync_ReturnsOkResult()
		{
			// Arrange
			var customers = new List<Customer> { new Customer { Id = 1, Name = "John Doe" } };
			_mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(customers);

			// Act
			var result = await _controller.GetCustomerListAsync();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<List<Customer>>(okResult.Value);
			Assert.Single(returnValue);
		}

		[Fact, Priority(2)]
		public async Task GetCustomerAsync_ReturnsNotFoundResult_WhenCustomerNotFound()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Customer)null);

			// Act
			var result = await _controller.GetCustomerAsync(1);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact, Priority(3)]
		public async Task GetCustomerAsync_ReturnsOkResult_WhenCustomerFound()
		{
			// Arrange
			var customer = new Customer { Id = 1, Name = "John Doe" };
			_mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(customer);

			// Act
			var result = await _controller.GetCustomerAsync(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<Customer>(okResult.Value);
			Assert.Equal(1, returnValue.Id);
		}

		[Fact, Priority(4)]
		public async Task CreateCustomerAsync_ReturnsCreatedAtActionResult()
		{
			// Arrange
			var customerDto = new CreateCustomerDTO { /* Initialize properties */ };
			var customer = new Customer { Id = 1, Name = "John Doe" };
			_mockMapper.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerDTO>())).Returns(customer);
			_mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.CreateCustomerAsync(customerDto);

			// Assert
			var createdAtActionResult = Assert.IsType<CreatedResult>(result.Result);
			var returnValue = Assert.IsType<Customer>(createdAtActionResult.Value);
			Assert.Equal(1, returnValue.Id);
		}

		[Fact, Priority(5)]
		public async Task UpdateCustomerAsync_ReturnsOkResult()
		{
			// Arrange
			var customerDto = new UpdateCustomerDTO { Id = 1, /* Initialize other properties */ };
			var customer = new Customer { Id = 1, Name = "John Doe" };
			_mockMapper.Setup(m => m.Map<Customer>(It.IsAny<UpdateCustomerDTO>())).Returns(customer);
			_mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.UpdateCustomerAsync(customerDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Successfully Updated", okResult.Value);
		}

		[Fact, Priority(6)]
		public async Task DeleteCustomerAsync_ReturnsOkResult()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.DeleteCustomerAsync(1);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("Sucessfully Deleted", okResult.Value);
		}
	}
}