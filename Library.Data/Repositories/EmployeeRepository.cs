using Library.Model.Enums;
using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{

    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }


    public override async Task<Employee?> GetById(Guid id, bool trackChanges, int languageId)
    {
        var stringId = id.ToString();
        return trackChanges ?
            await _context.Employees.FirstOrDefaultAsync(x => x.Id == stringId) :
            await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == stringId);
    }


    public async Task<Employee?> GetByEmailAsync(string email)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.NormalizedEmail!.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        return employee;
    }
}
