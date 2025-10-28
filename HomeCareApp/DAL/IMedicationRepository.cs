using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public interface IMedicationRepository
{
	Task<IEnumerable<Medication>?> GetAll();
    Task<Medication?> GetMedById(int id);
	Task<bool> Create(Medication medication);
    Task<bool> Update(Medication medication);
    Task<bool> Delete(int id);
}
