using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public class EmployeeRepository : IEmployeeRepository


{ // Repository for CRUD operations on employee entities
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db)
    {
        _db = db;

        // helps with debugging the DB path
        var path = _db.Database.GetDbConnection().DataSource;
        Console.WriteLine($"DB PATH = {path}");
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        // AsNoTracking improves performance when entities aren't updated
        return await _db.Employees.AsNoTracking().ToListAsync();  
    }

    public async Task<Employee?> GetItemById(int id)
    {
        return await _db.Employees.FindAsync(id);
    }

    public async Task Create(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Employee employee)
    {
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync();
    }

    // delete employee by id, returnes true if successful and false if not
    public async Task<bool> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
        {
            return false;
        }

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return true;
    }

}