using System.Threading.Tasks;
using HomeCareApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCareApp.DAL;

    

       /* public AppointmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Create(Appointment appointment)
        {
            // Sikkerhetsnett: sørg for at vi bare lagrer via FK-verdier.
            // Hvis noen skulle ha fylt inn navigasjoner utilsiktet, ikke attach dem.
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

            // Tasks ved booking via dette skjemaet er normalt tomt.
            // Hvis du i fremtiden poster tasks samtidig: sørg for at AppointmentId settes etter SaveChanges.
            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }*/
       public interface IAppointmentRepository
    {
        Task Create(Appointment appointment);
        Task<Appointment?> Get(int id);
        Task<List<Appointment>> GetForPatient(int patientId);
        Task<List<Appointment>> GetForEmployee(int employeeId);
        Task<List<DateTime>> GetAvailableDays(int daysAhead = 30);
   
        // legg til flere metoder hvis du har behov
    }
    

