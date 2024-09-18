using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.Repository;
using AutoMapper;
using CallCenterManagementAPI.DTO;
using CallCenterManagementAPI.Interface;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterManagementAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AgentsController : ControllerBase
	{
		private readonly IRepository<Agent> _repo;
		private readonly ILogger<AgentsController> _logger;
		private readonly IMapper _mapper;
		public AgentsController(IRepository<Agent> repo, IMapper mapper, ILogger<AgentsController> logger)
		{
			_repo = repo;
			_mapper = mapper;
			_logger = logger;
		}

		// GET: api/Agents
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Agent>>> GetAgentListAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			_logger.LogInformation("Getting all agents");
			var agents = await _repo.GetAllAsync(pageNumber, pageSize);
			return Ok(agents);
		}

		// GET: api/Agents/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Agent>> GetAgentAsync(int id)
		{
			_logger.LogInformation($"Getting agent with ID {id}");
			var agent = await _repo.GetByIdAsync(id);

			if (agent == null)
			{
				_logger.LogWarning($"Agent with ID {id} not found");
				return NotFound();
			}

			return Ok(agent);
		}

		// PUT: api/Agents/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut]
		public async Task<IActionResult> UpdateAgent(UpdateAgentDTO agentDto)
		{
			_logger.LogInformation($"Updating running activity with ID {agentDto.Id}");
			var agent = _mapper.Map<Agent>(agentDto);
			await _repo.UpdateAsync(agent);
			_logger.LogInformation($"Updated");
			return Ok("Successfully Updated");
		}

		[HttpPut("UpdateStatus")]
		public async Task<IActionResult> UpdateAgentStatus(UpdateAgentStatusDTO updateStatusDto)
		{
			_logger.LogInformation($"Updating status for agent with ID {updateStatusDto.Id}");
			var agent = await _repo.GetByIdAsync(updateStatusDto.Id);

			if (agent == null)
			{
				_logger.LogWarning($"Agent with ID {updateStatusDto.Id} not found");
				return NotFound();
			}

			agent.Status = updateStatusDto.Status;
			await _repo.UpdateAsync(agent);
			_logger.LogInformation($"Updated status for agent with ID {updateStatusDto.Id}");
			return Ok("Successfully Updated Status");
		}

		// POST: api/Agents
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Agent>> CreateAgent(CreateAgentDTO agentDTO)
		{
			_logger.LogInformation("Adding a new agent");
			var agent = _mapper.Map<Agent>(agentDTO);
			agent.Status = Enums.AgentStatus.Available;
			await _repo.AddAsync(agent);

			_logger.LogInformation("Successfully added");
			return Created("/api/agents/" + agent.Id, agent);
		}

		// DELETE: api/Agents/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAgent(int id)
		{
			_logger.LogInformation($"Deleting agent with ID {id}");
			await _repo.DeleteAsync(id);
			_logger.LogInformation($"Deleted");

			return Ok("Sucessfully Deleted");
		}
	}
}
