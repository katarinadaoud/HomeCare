using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeCareApp.Models;

public class AppDbContext : IdentityDbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<Admin> Admins { get; set; }
	public DbSet<AdminLog> AdminLogs { get; set; }
	public DbSet<Appointment> Appointments { get; set; }
	public DbSet<Task> Tasks { get; set; }
	public DbSet<AvailableDay> AvailableDays { get; set; }
	public DbSet<EmergencyContact> EmergencyContacts { get; set; }
	public DbSet<EmergencyCall> EmergencyCalls { get; set; }
	
}