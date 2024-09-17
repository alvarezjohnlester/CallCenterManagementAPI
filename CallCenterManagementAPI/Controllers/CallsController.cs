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
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.DTO;

namespace CallCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	
	public class CallsController : ControllerBase
    {
		private readonly IRepository<Call> _repo;
		private readonly ILogger<CallsController> _logger;
		private readonly IMapper _mapper;
		public CallsController(IRepository<Call> repo, IMapper mapper, ILogger<CallsController> logger)
        {
			_repo = repo;
			_mapper = mapper;
			_logger = logger;
		}

		// GET: api/Calls
		[HttpGet]
		[AllowAnonymous] // This allows unauthenticated access to this endpoint
		public async Task<ActionResult<IEnumerable<Call>>> GetCallListAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			_logger.LogInformation("Getting all calls");
			var call = await _repo.GetAllAsync(pageNumber, pageSize);
			return Ok(call);
		}

		// GET: api/Calls/5
		[HttpGet("{id}")]
		[AllowAnonymous] // This allows unauthenticated access to this endpoint
		public async Task<ActionResult<Call>> GetCallAsync(int id)
        {
			_logger.LogInformation($"Getting call with ID {id}");
			var call = await _repo.GetByIdAsync(id);

			if (call == null)
			{
				_logger.LogWarning($"Call with ID {id} not found");
				return NotFound();
			}

			return Ok(call);
		}

        // PUT: api/Calls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateCallAsync(UpdateCallDTO callDto)
        {
			_logger.LogInformation($"Updating call with ID {callDto.Id}");
			var call = _mapper.Map<Call>(callDto);
			await _repo.UpdateAsync(call);
			_logger.LogInformation($"Updated");
			return Ok("Successfully Updated");
		}

        // POST: api/Calls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		[Authorize]
		public async Task<ActionResult<Call>> CreateCall(CreateCallDTO callDTO)
        {
			_logger.LogInformation("Adding a new call");
			var Call = _mapper.Map<Call>(callDTO);
			Call.Status = Enums.CallStatus.Queued;
			await _repo.AddAsync(Call);

			_logger.LogInformation("Successfully added");
			return Created("/api/calls/" + Call.Id, Call);
		}

        // DELETE: api/Calls/5
        [HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteCall(int id)
        {
			_logger.LogInformation($"Deleting call with ID {id}");
			await _repo.DeleteAsync(id);
			_logger.LogInformation($"Deleted");

			return Ok("Sucessfully Deleted");
		}
    }
}
