using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Model;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.DTO;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class TicketsController : ControllerBase
    {
		private readonly IRepository<Ticket> _repo;
		private readonly ILogger<TicketsController> _logger;
		private readonly IMapper _mapper;
		public TicketsController(IRepository<Ticket> repo, IMapper mapper, ILogger<TicketsController> logger)
        {
			_repo = repo;
			_mapper = mapper;
			_logger = logger;
		}

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketListAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
			_logger.LogInformation("Getting all Tickets");
			var ticket = await _repo.GetAllAsync(pageNumber, pageSize);
			return Ok(ticket);
		}

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketAsync(int id)
        {
			_logger.LogInformation($"Getting Ticket with ID {id}");
			var ticket = await _repo.GetByIdAsync(id);

			if (ticket == null)
			{
				_logger.LogWarning($"Ticket with ID {id} not found");
				return NotFound();
			}

			return Ok(ticket);
		}

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateTicketAsync(UpdateTicketDTO ticketDto)
        {
			_logger.LogInformation($"Updating Ticket with ID {ticketDto.Id}");
			var tempticket = await _repo.GetByIdAsync(ticketDto.Id);
			if (tempticket == null)
			{
				return NotFound();
			}
			var ticket = _mapper.Map<Ticket>(ticketDto);
			ticket.UpdatedAt = DateTime.Now;
			ticket.CreatedAt= tempticket.CreatedAt;

			await _repo.UpdateAsync(ticket);
			_logger.LogInformation($"Updated");
			return Ok("Successfully Updated");
		}

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicketAsync(CreateTicketDTO ticketDto)
        {
			_logger.LogInformation("Adding a new ticket");
			var ticket = _mapper.Map<Ticket>(ticketDto);
			ticket.CreatedAt = DateTime.Now;
			ticket.Status = Enums.TicketStatus.Open;

			await _repo.AddAsync(ticket);

			_logger.LogInformation("Successfully added");
			return Created("/api/tickets/" + ticket.Id, ticket);
		}

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketAsync(int id)
        {
			_logger.LogInformation($"Deleting ticket with ID {id}");
			await _repo.DeleteAsync(id);
			_logger.LogInformation($"Deleted");

			return Ok("Sucessfully Deleted");
		}
    }
}
