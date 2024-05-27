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


    public override async Task<Employee?> GetById(Guid id)
    {
        var stringId = id.ToString();
        var employee = await _context.Employees.FindAsync(stringId);

        return employee;
    }


    public async Task<Employee?> GetByEmailAsync(string email)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper());
        return employee;
    }
}
