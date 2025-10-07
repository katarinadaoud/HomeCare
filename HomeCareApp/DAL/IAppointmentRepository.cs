using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public interface IAppointmentRepository
{
    Task Create(Appointment appointment);
    Task<IEnumerable<Appointment>> GetAll();
}