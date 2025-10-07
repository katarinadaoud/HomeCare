using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAll();
    Task Create(Patient patient);
}
