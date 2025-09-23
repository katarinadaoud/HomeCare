using Microsoft.EntityFrameworkCore;

namespace HomeCareApp.Models;

public class UserDbContext : DbContext
{
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
        Database.EnsureCreated();
	}

	public DbSet<User> Users { get; set; }
}