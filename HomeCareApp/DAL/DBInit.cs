using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HomeCareApp.DAL;


public static class DBInit
{
    /*public static void Seed(WebApplication app) //her lager vi en metode som skal kalle migrate og seed data
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.Migrate();


        //Legger inn en eksempel-attributt for alle entiteter slik at databasen kan testes

        //Users
        var users = new List<User>
        {
            new User { 
                UserName = "patient1",
                Email = "patient1@test.com"},
            new User {
                UserName = "employee1",
                Email = "employee1@test.com"},
            new User {
                UserName = "admin1",
                Email = "admin1@test.com",}
        };
            db.Users.AddRange(users);
        db.SaveChanges();

        //EmergencyContact
        var emergencycontacts = new List<EmergencyContact>
        {
            new EmergencyContact
            {
                Name = "Marit Larsen",
                Phone = "12345678",
                Email = "marit@larsen.com",
                PatientRelation = "Wife",
                Patient = patients[0]
            }
        };
        db.EmergencyContacts.AddRange(emergencycontacts);
        db.SaveChanges();


        //Patients
        var patients = new List<Patient>
        {
            new Patient {
                FullName = "Tor Hansen",
                Address = "Storgata 1, 0181 Oslo",
                HealthRelated_info = "Dementia",
                UserId = users[0].Id
                }
        };
        db.Patients.AddRange(patients);
        db.SaveChanges();

        
        //Employees
        var employees = new List<Employee>
        {
            new Employee
            {
                FullName = "Ida Johansen",
                Address = "Solveien 6",
                Department = "Oslo",
                UserId = users[1].Id
            }
        };
        db.Employees.AddRange(employees);
        db.SaveChanges();

        //Appointments
        var appointments = new List<Appointment>
        {
            new Appointment
            {
                Subject = "Control appointment",
                Description = "Yearly check",
                Date = DateTime.Today.AddDays(3),
                Patient = patients[0],
                Employee = employees[0],

                //AppointmentTasks
                AppointmentTasks = new List<AppointmentTask>
                {
                    new AppointmentTask { Description = "Take a blood test", Status = "Pending" },
                    new AppointmentTask { Description = "Measure blood pressure", Status = "Pending" }
                }
            }
        };
        db.Appointments.AddRange(appointments);
        db.SaveChanges();

        //Notifications
        var notifications = new List<Notification>
        {
            new Notification
            {
                Patient = patients[0],
                Message = "New appointment was created",
                CreatedAt = DateTime.Now,
                IsRead = false
            }
        };
        db.Notifications.AddRange(notifications);
        db.SaveChanges();

        //EmergencyCalls
        var emergencycalls = new List<EmergencyCall>
        {
            new EmergencyCall
            {
                Time = DateTime.Now,
                Status = "Open",
                Employee = employees[0],
                Patient = patients[0]
            }
        };
        db.EmergencyCalls.AddRange(emergencycalls);
        db.SaveChanges();

        //Admins
        var admins = new List<Admin>
        {
            new Admin
            {
                Accesses = "Full",
                UserId = users[2].Id,
                AdminLogs = new List<AdminLog>
                {
                    new AdminLog { Action = "Created initial seed data", Time = DateTime.Now},
                    new AdminLog { Action = "Checked system health", Time = DateTime.Now.AddMinutes(5) }

                }
            }
        };
        db.Admins.AddRange(admins);
        db.SaveChanges();   
            

    }*/
    // DAL/DBInit.cs

        public static void Seed(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Kjør migrasjoner
            db.Database.Migrate();

            // Idempotent vakt – hvis Users finnes, antar vi at seeding er kjørt
            if (db.Users.Any()) return;

            using var tx = db.Database.BeginTransaction();

            // 1) Users (Identity-brukere; enkel demo-seed uten passord)
            var users = new List<User>
            {
                new() { UserName = "patient1",  Email = "patient1@test.com"  },
                new() { UserName = "employee1", Email = "employee1@test.com" },
                new() { UserName = "admin1",    Email = "admin1@test.com"    }
            };
            db.Users.AddRange(users);
            db.SaveChanges();

            // 2) EmergencyContact – MÅ komme før Patient (FK ligger på Patient)
            var emergencyContact = new EmergencyContact
            {
                Name = "Marit Larsen",
                Phone = "12345678",
                Email = "marit@larsen.com",
                PatientRelation = "Wife"
            };
            db.EmergencyContacts.Add(emergencyContact);
            db.SaveChanges();

            // 3) Patient – peker til EmergencyContact (FK på Patient, unik 1–1 jf. migrasjon)
            var patient = new Patient
            {
                FullName = "Tor Hansen",
                Address = "Storgata 1, 0181 Oslo",
                HealthRelated_info = "Dementia",
                UserId = users[0].Id,                       // Identity FK (string)
                EmergencyContactId = emergencyContact.EmergencyContactId
            };
            db.Patients.Add(patient);

            // 4) Employee – peker til User
            var employee = new Employee
            {
                FullName = "Ida Johansen",
                Address = "Solveien 6",
                Department = "Oslo",
                UserId = users[1].Id
            };
            db.Employees.Add(employee);
            db.SaveChanges();

            // 5) Appointment (+ Tasks) – bruk FK-feltene eksplisitt
            var appt = new Appointment
            {
                Subject = "Control appointment",
                Description = "Yearly check",
                Date = DateTime.Today.AddDays(3), // lagres som DATE pga [Column(TypeName="date")]
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                AppointmentTasks = new List<AppointmentTask>
                {
                    new() { Description = "Take a blood test",       Status = "Pending" },
                    new() { Description = "Measure blood pressure",   Status = "Pending" }
                }
            };
            db.Appointments.Add(appt);
            db.SaveChanges();

            // 6) Notification – knytt til pasient
            db.Notifications.Add(new Notification
            {
                Patient = patient,
                Message = "New appointment was created",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            });

            // 7) EmergencyCall
            db.EmergencyCalls.Add(new EmergencyCall
            {
                Time = DateTime.UtcNow,
                Status = "Open",
                Employee = employee,
                Patient = patient
            });

            // 8) Admin + logs
            db.Admins.Add(new Admin
            {
                Accesses = "Full",
                UserId = users[2].Id,
                AdminLogs = new List<AdminLog>
                {
                    new() { Action = "Created initial seed data", Time = DateTime.UtcNow },
                    new() { Action = "Checked system health",     Time = DateTime.UtcNow.AddMinutes(5) }
                }
            });

            db.SaveChanges();
            tx.Commit();
        }
    }

