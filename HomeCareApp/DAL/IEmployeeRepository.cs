using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee?> GetItemById(int id);
    Task Create(Employee employee);
    Task Update(Employee employee);
    Task<bool> Delete(int id);
}
