using Newtonsoft.Json;
using System.Net;

namespace CallCenterManagementAPI.Middleware
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<CustomExceptionMiddleware> _logger;

		public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong: {ex}");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var response = new
			{
				StatusCode = context.Response.StatusCode,
				Message = exception.Message
			};

			return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
		}

	}
}
