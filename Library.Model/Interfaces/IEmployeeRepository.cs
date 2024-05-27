using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee?> GetByEmailAsync(string email);
}
