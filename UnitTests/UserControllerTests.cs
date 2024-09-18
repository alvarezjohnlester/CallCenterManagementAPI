
using AutoMapper;
using CallCenterManagementAPI.Controllers;
using CallCenterManagementAPI.DTO;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
	public class UserControllerTests
	{
		private readonly Mock<IUserRepository> _mockUserRepository;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<IConfiguration> _mockConfiguration;
		private readonly TokenService _tokenService;
		private readonly UserController _controller;

		public UserControllerTests()
		{
			_mockUserRepository = new Mock<IUserRepository>();
			_mockMapper = new Mock<IMapper>();
			_mockConfiguration = new Mock<IConfiguration>();

			// Setup configuration mock
			_mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("your_secret_key_heretest123123longkey");
			_mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("your_issuer_here");
			_mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("your_audience_here");

			_tokenService = new TokenService(_mockConfiguration.Object);
			_controller = new UserController(_mockUserRepository.Object, _mockMapper.Object, _tokenService);
		}

		[Fact, Priority(1)]
		public async Task Register_UserAlreadyExists_ReturnsBadRequest()
		{
			// Arrange
			var userDto = new CreateUserDTO { Username = "existingUser" };
			var user = new User { Username = "existingUser" };
			_mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);
			_mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(user.Username)).ReturnsAsync(user);

			// Act
			var result = await _controller.Register(userDto);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("Username already exists.", badRequestResult.Value);
		}

		[Fact, Priority(2)]
		public async Task Register_NewUser_ReturnsOk()
		{
			// Arrange
			var userDto = new CreateUserDTO { Username = "newUser" };
			var user = new User { Username = "newUser" };
			_mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);
			_mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(user.Username)).ReturnsAsync((User)null);
			_mockUserRepository.Setup(repo => repo.AddUserAsync(user)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.Register(userDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal("User registered successfully.", okResult.Value);
		}

		[Fact, Priority(3)]
		public async Task Login_InvalidCredentials_ReturnsUnauthorized()
		{
			// Arrange
			var loginDto = new LogInDTO { Username = "invalidUser", Password = "wrongPassword" };
			_mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(loginDto.Username)).ReturnsAsync((User)null);

			// Act
			var result = await _controller.Login(loginDto);

			// Assert
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
		}

		[Fact, Priority(4)]
		public async Task Login_ValidCredentials_ReturnsOkWithToken()
		{
			// Arrange
			var loginDto = new LogInDTO { Username = "validUser", Password = "correctPassword" };
			var user = new User { Username = "validUser", Password = "correctPassword" };
			_mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

			// Act
			var result = await _controller.Login(loginDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var tokenResponse = Assert.IsType<TokenResponse>(okResult.Value);
			Assert.NotNull(tokenResponse.Token);
		}
	}
}




