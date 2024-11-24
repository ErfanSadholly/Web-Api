using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Api.Account_Models;
using Web_Api.DbModels;

namespace Web_Api.AppData
{
	public class AppDbContext : IdentityDbContext<User, Role, int>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<PhoneBook> PhoneBooks { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<PhoneBook>().HasQueryFilter(p => !p.Deleted);
		}
	}
}
