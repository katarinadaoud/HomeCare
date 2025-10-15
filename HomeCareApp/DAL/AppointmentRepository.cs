using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AppointmentRepository> _logger;

        public AppointmentRepository(AppDbContext db, ILogger<AppointmentRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Appointment>> GetAll()
        {
            try
            {
                return await _db.Appointments
                   /* .Include(a => a.Patient)
                    .Include(a => a.Employee)*/
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AppointmentRepository] GetAll() failed: {Message}", ex.Message);
                return new List<Appointment>();
            }
        }

        public async Task<Appointment?> GetAppointmentById(int id)
        {
            try
            {
                return await _db.Appointments.FindAsync(id);
                    /*.Include(a => a.Patient)
                    .Include(a => a.Employee)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);*/
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AppointmentRepository] Get({Id}) failed: {Message}", id, ex.Message);
                return null;
            }
        }

        public async Task<bool> Create(Appointment appointment)
        {
            try
            {
                /*if (appointment.Patient != null)
                {
                    _db.Entry(appointment.Patient).State = EntityState.Detached;
                    appointment.Patient = null;
                }
                if (appointment.Employee != null)
                {
                    _db.Entry(appointment.Employee).State = EntityState.Detached;
                    appointment.Employee = null;
                }*/

                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AppointmentRepository] Create() failed: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(Appointment appointment)
        {
            try
            {
                _db.Appointments.Update(appointment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AppointmentRepository] Update() failed: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var appointment = await _db.Appointments.FindAsync(id);
                if (appointment == null)
                    return false;

                _db.Appointments.Remove(appointment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AppointmentRepository] Delete({Id}) failed: {Message}", id, ex.Message);
                return false;
            }
        }

        /*public async Task<List<Appointment>> GetForPatient(int patientId)
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
        }*/
    }
}
