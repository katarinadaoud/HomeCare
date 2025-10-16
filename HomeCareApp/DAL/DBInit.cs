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
            new User
            {
                UserName = "patient1",
                Email = "patient1@test.com",
                Admins = new List<Admin>(),
                Employees = new List<Employee>(),
                Patients = new List<Patient>()
            },
            new User
            {
                UserName = "employee1",
                Email = "employee1@test.com",
                Admins = new List<Admin>(),
                Employees = new List<Employee>(),
                Patients = new List<Patient>()
            },
            new User
            {
                UserName = "admin1",
                Email = "admin1@test.com",
                Admins = new List<Admin>(),
                Employees = new List<Employee>(),
                Patients = new List<Patient>()
            }
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
                UserId = people.First(u => u.UserName == "patient1").Id,
                // Set the navigation to the existing user (non-nullable), initialize collections,
                // and use null-forgiving for EmergencyContact if you plan to link it later.
                User = people.First(u => u.UserName == "patient1"),
                EmergencyContact = null!,
                Appointments = new List<Appointment>(),
                EmergencyCalls = new List<EmergencyCall>()
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
                PatientRelation = "Wife",
                Patient = db.Patients.First(p => p.FullName == "Tor Hansen")
                // FK is effectively on the Patient side, but the model requires the navigation property to be set
            }
        };
            db.EmergencyContacts.AddRange(emergencyContacts);
            db.SaveChanges();

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
                UserId = people.First(u => u.UserName == "employee1").Id,
                // set the navigation to the existing user and initialize required collections
                User = people.First(u => u.UserName == "employee1"),
                Appointments = new List<Appointment>(),
                AvailableDays = new List<AvailableDay>(),
                EmergencyCalls = new List<EmergencyCall>()
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
            // Create the appointment first so we can set the required Appointment navigation on each task
            var appointment1 = new Appointment
            {
                Subject = "Control appointment",
                Description = "Yearly check-up and health review",
                Date = DateTime.Today.AddDays(3),
                Patient = ppeople.First(p => p.FullName == "Tor Hansen"),
                PatientId = ppeople.First(p => p.FullName == "Tor Hansen").PatientId,
                Employee = epeople.First(e => e.FullName == "Ida Johansen"),
                EmployeeId = epeople.First(e => e.FullName == "Ida Johansen").EmployeeId,
                AppointmentTasks = new List<AppointmentTask>()
            };

            var task1 = new AppointmentTask { Description = "Take a blood test",     Status = "Pending", Appointment = appointment1 };
            var task2 = new AppointmentTask { Description = "Measure blood pressure", Status = "Pending", Appointment = appointment1 };

            appointment1.AppointmentTasks.Add(task1);
            appointment1.AppointmentTasks.Add(task2);

            var appointments = new List<Appointment> { appointment1 };

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
                // set required navigation properties to existing patient and employee
                Employee = epeople.First(e => e.FullName == "Ida Johansen"),
                Patient = ppeople.First(p => p.FullName == "Tor Hansen")
            }
        };
            db.EmergencyCalls.AddRange(emergencyCalls);
            db.SaveChanges();


        }
        
        // ---------- ADMINS ----------
        if (!db.Admins.Any())
        {
           var admin = new Admin
           {
               Accesses = "Full",
               UserId = people.First(u => u.UserName == "admin1").Id,
               User = people.First(u => u.UserName == "admin1"),
               AdminLogs = new List<AdminLog>()
           };

           // Create logs and set the required Admin navigation property
           admin.AdminLogs.Add(new AdminLog { Action = "Created initial seed data", Time = DateTime.Now, Admin = admin });
           admin.AdminLogs.Add(new AdminLog { Action = "Checked system health",     Time = DateTime.Now.AddMinutes(5), Admin = admin });

           var admins = new List<Admin> { admin };
           db.Admins.AddRange(admins);
           db.SaveChanges(); 
        }
        
    }
}
