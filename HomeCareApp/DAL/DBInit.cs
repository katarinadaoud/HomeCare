using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
//her skal det være feks patients, employees, appointments osvosv
    }
}