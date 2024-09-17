using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Model;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.DTO;

namespace CallCenterManagementAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CustomersController : ControllerBase
	{
		private readonly IRepository<Customer> _repo;
		private readonly ILogger<CustomersController> _logger;
		private readonly IMapper _mapper;

		public CustomersController(IRepository<Customer> repo, IMapper mapper, ILogger<CustomersController> logger)
		{
			_repo = repo;
			_mapper = mapper;
			_logger = logger;
		}

		// GET: api/Customers
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerListAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			_logger.LogInformation("Getting all customers");
			var customer = await _repo.GetAllAsync(pageNumber, pageSize);
			return Ok(customer);
		}

		// GET: api/Customers/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Customer>> GetCustomer(int id)
		{
			_logger.LogInformation($"Getting Customer with ID {id}");
			var customer = await _repo.GetByIdAsync(id);

			if (customer == null)
			{
				_logger.LogWarning($"Call with ID {id} not found");
				return NotFound();
			}

			return Ok(customer);
		}

		// PUT: api/Customers/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut]
		public async Task<IActionResult> UpdateCustomer(UpdateCustomerDTO customerDTO)
		{
			_logger.LogInformation($"Updating Customer with ID {customerDTO.Id}");
			var customer = _mapper.Map<Customer>(customerDTO);
			await _repo.UpdateAsync(customer);
			_logger.LogInformation($"Updated");
			return Ok("Successfully Updated");
		}

		// POST: api/Customers
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Customer>> CreateCustomer(CreateCustomerDTO customerDTO)
		{
			_logger.LogInformation("Adding a new customer");
			var customer = _mapper.Map<Customer>(customerDTO);
			await _repo.AddAsync(customer);

			_logger.LogInformation("Successfully added");
			return Created("/api/customers/" + customer.Id, customer);
		}

		// DELETE: api/Customers/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(int id)
		{
			_logger.LogInformation($"Deleting customer with ID {id}");
			await _repo.DeleteAsync(id);
			_logger.LogInformation($"Deleted");

			return Ok("Sucessfully Deleted");

		}
	}
}
