using HomeCareApp.Models;

namespace HomeCareApp.DAL;

// basic crud for patient entities
public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAll();
    Task<Patient?> GetItemById(int id);
    Task Create(Patient patient);
<<<<<<< HEAD

    Task Update(Patient patient); // updates patient
    Task<bool> Delete(int id); // deletes patient by id
=======
    Task Update(Patient patient);
    Task<bool> Delete(int id);
>>>>>>> e42e3a3d3a827b2ffc5122b206670b784f2e03d7
}
