using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;
//using Castle.Components.DictionaryAdapter;

namespace HomeCareApp.DAL;

public class AppDbContext : IdentityDbContext<User>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{ }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<Admin> Admins { get; set; }
	public DbSet<AdminLog> AdminLogs { get; set; }
	public DbSet<Appointment> Appointments { get; set; }
	public DbSet<HomeCareApp.Models.AppointmentTask> Tasks { get; set; }
	public DbSet<AvailableDay> AvailableDays { get; set; }
	public DbSet<EmergencyContact> EmergencyContacts { get; set; }
	public DbSet<EmergencyCall> EmergencyCalls { get; set; }
	public DbSet<Notification> Notifications { get; set; }
}