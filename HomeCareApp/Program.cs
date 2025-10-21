using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using HomeCareApp.DAL;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:AppDbContextConnection"]);
});

//builder.Services.AddDefaultIdentity<User>() (we dont need both this and the one below)
   // .AddEntityFrameworkStores<AppDbContext>()
   // .AddDefaultTokenProviders();

builder.Services.AddDefaultIdentity<User>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 6;

    /* Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true; */

    // User settings
    options.User.RequireUniqueEmail = true;

    // Sign-in settings
    options.SignIn.RequireConfirmedAccount = false; // Set to true if email confirmation is implemented
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; //Path for the login page
});

builder.Services.AddRazorPages();
//builder.Services.AddSession(); (dont need this line if we are using the options below)
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".HomeCareApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 minutes
    options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information() // levels: Trace< Information < Warning < Erorr < Fatal
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

    loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    await DBInit.Seed(app);
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();