using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeCareApp.Models;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
		// må slettes fordi den gjør at den hopper over migrasjoner
		// Database.EnsureCreated();
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<Admin> Admins { get; set; }
	public DbSet<AdminLog> AdminLogs { get; set; }
	public DbSet<Appointment> Appointments { get; set; }
	// endrer denne: public DbSet<Task> Tasks { get; set; }
	//til dette:
	// AppDbContext.cs
	public DbSet<HomeCareApp.Models.Task> Tasks { get; set; }
	//fordi det kræsjer med System.Threading.Tasks

	public DbSet<AvailableDay> AvailableDays { get; set; }
	public DbSet<EmergencyContact> EmergencyContacts { get; set; }
	public DbSet<EmergencyCall> EmergencyCalls { get; set; }
	
}