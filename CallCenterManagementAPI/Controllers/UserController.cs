using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.Service;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.DTO;
using AutoMapper;

namespace CallCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
		private readonly IUserRepository _userRepository;
		private readonly TokenService _tokenService;

		private readonly IMapper _mapper;
		public UserController(IUserRepository userRepository, IMapper mapper, TokenService tokenService)
		{
			_userRepository = userRepository;
			_tokenService = tokenService;
			_mapper = mapper;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] CreateUserDTO userDto)
		{
			var user = _mapper.Map<User>(userDto);
			var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
			if (existingUser != null)
			{
				return BadRequest("Username already exists.");
			}

			await _userRepository.AddUserAsync(user);
			return Ok("User registered successfully.");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LogInDTO login)
		{
			var user = await _userRepository.GetUserByUsernameAsync(login.Username);
			if (user == null || user.Password != login.Password)
			{
				return Unauthorized("Invalid username or password.");
			}

			var token = _tokenService.GenerateToken(user.Username);
			TokenResponse tokenResponse = new TokenResponse();
			tokenResponse.Token = token;
			return Ok(tokenResponse);
		}
	}
}
