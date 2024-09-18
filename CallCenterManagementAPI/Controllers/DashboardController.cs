using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallCenterManagementAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class DashboardController : ControllerBase
	{
		//TO-do 
		//call per agent
		// average call
		//
		private readonly ICallRoutingService _callRoutingService;
		private readonly IRepository<Call> _callRepository;
		private readonly ILogger<DashboardController> _logger;
		public DashboardController(ICallRoutingService callRoutingService, IRepository<Call> callRepository, ILogger<DashboardController> logger)
		{
			_callRoutingService = callRoutingService;
			_logger = logger;
		}
		// POST: api/Dashboard/RouteCallToAgent
		[HttpPost("RouteCallToAgent")]
		public async Task<IActionResult> AssignCallToAgent([FromQuery] int callId)
		{
			_logger.LogInformation($"Assigning call with ID {callId} to an agent");
			var assignedAgent = await _callRoutingService.AssignCallToAgentAsync(callId);

			if (assignedAgent == null)
			{
				_logger.LogWarning("No available agents to assign the call or call not found");
				return NotFound("No available agents or call not found");
			}

			_logger.LogInformation($"Call with ID {callId} assigned to agent with ID {assignedAgent.Id}");
			return Ok($"Call assigned to agent {assignedAgent.Id}");
		}
	}
}


