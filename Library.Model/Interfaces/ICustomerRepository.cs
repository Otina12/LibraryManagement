using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface ICustomerRepository : IBaseModelRepository<Customer>
{
    Task<Customer?> GetById(string Id, bool trackChanges = false);
}
