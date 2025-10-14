using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    /*public async Task Create(Appointment appointment)
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
}*/


    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

        // === Påkrevd av IAppointmentRepository ===

        public async Task Create(Appointment appointment)
        {
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

        // === Ekstra / hjelpetjenester (valgfritt å bruke) ===

        public async Task<List<Appointment>> GetAll()
        {
            return await _db.Appointments
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Returnerer en liste med datoer (hverdager) de neste X dagene som IKKE har noen avtaler.
        /// </summary>
        public async Task<List<DateTime>> GetAvailableDays(int daysAhead = 30)
        {
            var today = DateTime.Today;
            var end = today.AddDays(daysAhead);

            // Hent alle datoer som allerede er booket i intervallet
            var bookedDates = await _db.Appointments
                .Where(a => a.Date.Date >= today && a.Date.Date <= end)
                .Select(a => a.Date.Date)
                .Distinct()
                .ToListAsync();

            // Alle datoer i intervallet
            var allDates = Enumerable.Range(0, daysAhead + 1)
                .Select(offset => today.AddDays(offset).Date);

            // Ledige = hverdager som ikke er i bookedDates
            var available = allDates
                .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                .Where(d => !bookedDates.Contains(d))
                .ToList();

            return available;
        }
}

