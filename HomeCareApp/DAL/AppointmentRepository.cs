using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;
    public AppointmentRepository(AppDbContext db) => _db = db;

    public async Task Create(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAll()
        => await _db.Appointments.ToListAsync();

    public async Task<IEnumerable<DateTime>> GetAvailableDays(int daysAhead = 30)
{
    var today = DateTime.Today;
    var end = today.AddDays(daysAhead);

    var bookedDates = await _db.Appointments
        .Where(a => a.Date >= today && a.Date <= end)
        .Select(a => a.Date.Date)
        .Distinct()
        .ToListAsync();

    var allDates = Enumerable.Range(0, daysAhead + 1)
        .Select(offset => today.AddDays(offset).Date);

    var available = allDates
        .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
        .Where(d => !bookedDates.Contains(d));

    return available;
}
}