using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HomeCareApp.DAL;

public static class DBInit
{
    public static void Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.Migrate();

        // Hopp over hvis vi allerede har data
        if (db.Users.Any()) return;

        // ---------- USERS ----------
        var users = new List<User>
        {
            new User { UserName = "patient1",  Email = "patient1@test.com"  },
            new User { UserName = "employee1", Email = "employee1@test.com" },
            new User { UserName = "admin1",    Email = "admin1@test.com"    }
        };
        db.Users.AddRange(users);
        db.SaveChanges();

        // ---------- PATIENTS ----------
        var patients = new List<Patient>
        {
            new Patient
            {
                FullName = "Tor Hansen",
                Address = "Storgata 1, 0181 Oslo",
                HealthRelated_info = "Dementia",
                UserId = users[0].Id
            }
        };
        db.Patients.AddRange(patients);
        db.SaveChanges();

        // ---------- EMERGENCY CONTACTS ----------
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
        patients[0].EmergencyContactId = emergencyContacts[0].EmergencyContactId;
        db.SaveChanges();

        // ---------- EMPLOYEES ----------
        var employees = new List<Employee>
        {
            new Employee
            {
                FullName = "Ida Johansen",
                Address = "Solveien 6, 1458 Oslo",
                Department = "Oslo",
                UserId = users[1].Id
            }
        };
        db.Employees.AddRange(employees);
        db.SaveChanges();

        // ---------- APPOINTMENTS ----------
        var appointments = new List<Appointment>
        {
            new Appointment
            {
                Subject = "Control appointment",
                Description = "Yearly check-up and health review",
                Date = DateTime.Today.AddDays(3),
                PatientId = patients[0].PatientId,
                EmployeeId = employees[0].EmployeeId,
                AppointmentTasks = new List<AppointmentTask>
                {
                    new AppointmentTask { Description = "Take a blood test",     Status = "Pending" },
                    new AppointmentTask { Description = "Measure blood pressure", Status = "Pending" }
                }
            }
        };
        db.Appointments.AddRange(appointments);
        db.SaveChanges();

        // ---------- NOTIFICATIONS ----------
        // Fjernet inntil tabell finnes (migrasjonen din 'AddNotifications' er tom)
        // db.Notifications.Add(new Notification { ... });
        // db.SaveChanges();

        // ---------- EMERGENCY CALLS ----------
        var emergencyCalls = new List<EmergencyCall>
        {
            new EmergencyCall
            {
                Time = DateTime.Now,
                Status = "Open",
                EmployeeId = employees[0].EmployeeId,
                PatientId = patients[0].PatientId
            }
        };
        db.EmergencyCalls.AddRange(emergencyCalls);
        db.SaveChanges();

        // ---------- ADMINS ----------
        var admins = new List<Admin>
        {
            new Admin
            {
                Accesses = "Full",
                UserId = users[2].Id,
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
