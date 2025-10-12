using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL{

public static class DBInit
{
    public static void Seed(WebApplication app) //her lager vi en metode som skal kalle migrate og seed data
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.Migrate();
        /*sletter dette fordi det vil slette databasen hver gang vi starter appen
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated(); */

        //her skal det være feks patients, employees, appointments osvosv
    }
}
}