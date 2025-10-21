using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;
// Repository for CRUD operations on patient entities
public class PatientRepository : IPatientRepository


{
    private readonly AppDbContext _db; // EF Core DbContext (injected via DI)


    public PatientRepository(AppDbContext db)
    {
        _db = db;

        /*Test*/
        var path = _db.Database.GetDbConnection().DataSource;
        Console.WriteLine($"DB PATH = {path}");
    }

    public async Task<IEnumerable<Patient>> GetAll()
    {
        return await _db.Patients.AsNoTracking().ToListAsync();
    }

    public async Task<Patient?> GetItemById(int id)
    {
        return await _db.Patients.FindAsync(id);
    }

    public async Task Create(Patient patient)
    {
        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Patient patient)
    {
        _db.Patients.Update(patient);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient == null)
        {
            return false;
        }

        _db.Patients.Remove(patient);
        await _db.SaveChangesAsync();
        return true;
    }

}