using HomeCareApp.Models;

namespace HomeCareApp.DAL;

// basic crud for patient entities
public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAll();
    Task<Patient?> GetItemById(int id);
    Task Create(Patient patient);
    Task Update(Patient patient);
    Task<bool> Delete(int id);
}
