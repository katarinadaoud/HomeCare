using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace HomeCareApp.DAL;

public static class DBInit
{
    public static async Task Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AppDbContext db = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureDeleted(); ;
        db.Database.EnsureCreated();

        // Hopp over hvis vi allerede har data
        if (!db.Users.Any())
        {
            // ---------- USERS ----------
            var users = new List<User>
        {
            new User { UserName = "patient1",  Email = "patient1@test.com"  },
            new User { UserName = "employee1", Email = "employee1@test.com" },
            new User { UserName = "admin1",    Email = "admin1@test.com"    }
        };
            db.Users.AddRange(users);
            db.SaveChanges();

        }
        
        var people = await db.Users.ToListAsync();
       

        // ---------- PATIENTS ----------
        if (!db.Patients.Any())
        {
            var patients = new List<Patient>
            {
            
            new Patient
            {
                FullName = "Tor Hansen",
                Address = "Storgata 1, 0181 Oslo",
                HealthRelated_info = "Dementia",
                UserId = people.FirstOrDefault(u => u.UserName == "patient1")?.Id            
            }
        };
            db.Patients.AddRange(patients);
            db.SaveChanges();

        }


        // ---------- EMERGENCY CONTACTS ----------
        if (!db.EmergencyContacts.Any())
        {
            var emergencyContacts = new List<EmergencyContact>
        {
            new EmergencyContact
            {
                Name = "Marit Larsen",
                Phone = "12345678",
                Email = "marit@larsen.com",
                PatientRelation = "Wife"
                // Viktig: IKKE sett .Patient her (FK-en er på Patient-siden)
            }
        };
            db.EmergencyContacts.AddRange(emergencyContacts);
            db.SaveChanges();

            // Knytt pasienten til kontakten (FK på Patient)
           // patients[0].EmergencyContactId = emergencyContacts[0].EmergencyContactId;
            //db.SaveChanges();

        }




        // ---------- EMPLOYEES ----------
        if (!db.Employees.Any())
        {
            var employees = new List<Employee>
        {
            new Employee
            {
                FullName = "Ida Johansen",
                Address = "Solveien 6, 1458 Oslo",
                Department = "Oslo",
                UserId = people.FirstOrDefault(u => u.UserName == "employee1")?.Id
            }
        };
            db.Employees.AddRange(employees);
            db.SaveChanges();


        }

                        var ppeople = await db.Patients.ToListAsync();
                        var epeople = await db.Employees.ToListAsync();



        // ---------- APPOINTMENTS ----------
        if (!db.Appointments.Any())
        {
            var appointments = new List<Appointment>
        {
            new Appointment
            {
                Subject = "Control appointment",
                Description = "Yearly check-up and health review",
                Date = DateTime.Today.AddDays(3),
                PatientId = ppeople.FirstOrDefault(p => p.FullName == "Tor Hansen")?.PatientId ?? 0,
                EmployeeId = epeople.FirstOrDefault(e => e.FullName == "Ida Johansen")?.EmployeeId ?? 0,

                AppointmentTasks = new List<AppointmentTask>
                {
                    new AppointmentTask { Description = "Take a blood test",     Status = "Pending" },
                    new AppointmentTask { Description = "Measure blood pressure", Status = "Pending" }
                }
            }
        };
            db.Appointments.AddRange(appointments);
            db.SaveChanges();

        }


        // ---------- NOTIFICATIONS ----------
        // Fjernet inntil tabell finnes (migrasjonen din 'AddNotifications' er tom)
        // db.Notifications.Add(new Notification { ... });
        // db.SaveChanges();

        // ---------- EMERGENCY CALLS ----------
        if (!db.EmergencyCalls.Any())
        {
            var emergencyCalls = new List<EmergencyCall>
        {
            new EmergencyCall
            {
                Time = DateTime.Now,
                Status = "Open",
                //EmployeeId = employees[0].EmployeeId,
                //PatientId = patients[0].PatientId
            }
        };
            db.EmergencyCalls.AddRange(emergencyCalls);
            db.SaveChanges();


        }
        
        // ---------- ADMINS ----------
        if (!db.Admins.Any())
        {
           var admins = new List<Admin>
        {
            new Admin
            {
                Accesses = "Full",
                //UserId = users[2].Id,
                AdminLogs = new List<AdminLog>
                {
                    new AdminLog { Action = "Created initial seed data", Time = DateTime.Now },
                    new AdminLog { Action = "Checked system health",     Time = DateTime.Now.AddMinutes(5) }
                }
            }
        };
        db.Admins.AddRange(admins);
        db.SaveChanges(); 
        }
        
    }
}
