using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UserDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:UserDbContextConnection"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();