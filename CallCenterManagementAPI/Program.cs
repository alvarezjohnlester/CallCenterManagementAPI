using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CallCenterManagementAPI.Data;
using System;
using CallCenterManagementAPI.Interface;
using CallCenterManagementAPI.Repository;
using CallCenterManagementAPI.Model;
using CallCenterManagementAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CallCenterManagementAPI.Service;
namespace CallCenterManagementAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<CallCenterManagementAPIContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			// Configure logging
			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();
			builder.Logging.AddDebug();

			builder.Services.AddScoped<IRepository<Agent>, AgentRepository>();
			builder.Services.AddScoped<IRepository<Call>, CallRepository>();
			builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
			builder.Services.AddScoped<IRepository<Ticket>, TicketRepository>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<TokenService>();
			// Add authentication services
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["Jwt:Issuer"],
						ValidAudience = builder.Configuration["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
					};
				});
			builder.Services.AddAuthorization();

			// Register AutoMapper
			builder.Services.AddAutoMapper(typeof(Program));
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<CustomExceptionMiddleware>();
			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();


			app.Run();
		}
	}
}
