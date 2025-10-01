using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAll();
    Task<Patient?> GetItemById(int id);
    System.Threading.Tasks.Task Create(Patient patient);
    System.Threading.Tasks.Task Update(Patient patient);
    Task<bool> Delete(int id);
}
