using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;
using Microsoft.EntityFrameworkCore.Internal;
using HomeCareApp.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:AppDbContextConnection"]);
});

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseAuthentication();

app.MapDefaultControllerRoute();

app.Run();