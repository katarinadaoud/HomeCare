using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee?> GetItemById(int id);
    System.Threading.Tasks.Task Create(Employee employee);
    System.Threading.Tasks.Task Update(Employee employee);
    Task<bool> Delete(int id);
}
