using HomeCareApp.Models;

namespace HomeCareApp.ViewModels
{
    public class EmployeesViewModel
    {
        public IEnumerable<Employee> Employees;
        public string? CurrentViewName;

        public EmployeesViewModel(IEnumerable<Employee> employees, string? currentViewName)
        {
            Employees = employees;
            CurrentViewName = currentViewName;
        }
    }
}