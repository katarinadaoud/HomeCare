using Microsoft.EntityFrameworkCore;

namespace HomeCareApp.Models;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Patients.Any())
        {
            var patients = new List<Patient>
            {
                new Patient{
                    PatientId = 0,
                    Address = "Bakkeveien 5, 1415 Oppegård",
                    HealthRelated_info = "Dementia, Needs help with exercise.",
                    EmergencyContactId = 5
                },
            };
            context.AddRange(patients);
            context.SaveChanges();
        }
    }
}