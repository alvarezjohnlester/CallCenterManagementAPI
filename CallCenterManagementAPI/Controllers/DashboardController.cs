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
		//call rerouting
		//

	}
}
