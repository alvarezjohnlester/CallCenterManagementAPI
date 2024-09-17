using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Data
{
    public class CallCenterManagementAPIContext : DbContext
    {
        public CallCenterManagementAPIContext (DbContextOptions<CallCenterManagementAPIContext> options)
            : base(options)
        {
        }

		public DbSet<Agent> Agent { get; set; } = default!;
		public DbSet<Call> Call { get; set; } = default!;
		public DbSet<Customer> Customer { get; set; } = default!;
		public DbSet<Ticket> Ticket { get; set; } = default!;
		public DbSet<User> Users { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Agent>().HasQueryFilter(a => !a.IsDeleted);
			modelBuilder.Entity<Call>().HasQueryFilter(c => !c.IsDeleted);
			modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
			modelBuilder.Entity<Ticket>().HasQueryFilter(t => !t.IsDeleted);
		}

		public DbSet<CallCenterManagementAPI.Model.User>? UserModel { get; set; }
	}
}
