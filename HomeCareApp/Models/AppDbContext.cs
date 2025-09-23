using Microsoft.EntityFrameworkCore;

namespace HomeCareApp.Models;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	public DbSet<User> Users { get; set; }
	//her skal det være alle lister med de forskjellige entitetene
}