using HomeCareApp.Models;


namespace HomeCareApp.DAL;
    // Defines basic CRUD operations for Appointment objects
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAll();
        Task<IEnumerable<Appointment>> GetAllForPatient(int? patientId);
        Task<Appointment?> GetAppointmentById(int id);
        Task<bool> Create(Appointment appointment);
        Task<bool> Update(Appointment appointment);
        Task<bool> Delete(int id);
        
    }


    

