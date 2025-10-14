using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task Create(Appointment appointment)
    {
        // Sikkerhetsnett: sørg for at vi bare lagrer via FK-verdier.
        if (appointment.Patient != null)
        {
            _db.Entry(appointment.Patient).State = EntityState.Detached;
            appointment.Patient = null;
        }
        if (appointment.Employee != null)
        {
            _db.Entry(appointment.Employee).State = EntityState.Detached;
            appointment.Employee = null;
        }

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();
    }

    public async Task<Appointment?> Get(int id)
    {
        return await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
    }

    public async Task<List<Appointment>> GetForPatient(int patientId)
    {
        return await _db.Appointments
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.Date)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetForEmployee(int employeeId)
    {
        return await _db.Appointments
            .Where(a => a.EmployeeId == employeeId)
            .OrderBy(a => a.Date)
            .ToListAsync();
    }

    public async Task<List<DateTime>> GetAvailableDays(int daysAhead = 30)
    {
        var today = DateTime.Today;
        var end = today.AddDays(daysAhead);

        var bookedDates = await _db.Appointments
            .Where(a => a.Date.Date >= today && a.Date.Date <= end)
            .Select(a => a.Date.Date)
            .Distinct()
            .ToListAsync();

        var allDates = Enumerable.Range(0, daysAhead + 1)
            .Select(offset => today.AddDays(offset).Date);

        var available = allDates
            .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
            .Where(d => !bookedDates.Contains(d))
            .ToList();

        return available;
    }
}


