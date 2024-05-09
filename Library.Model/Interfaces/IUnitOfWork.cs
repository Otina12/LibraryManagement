namespace Library.Model.Interfaces;

public interface IUnitOfWork // this will help use repositories and update made changes as 1 big chunk
{
    IEmployeeRepository Employees { get; }

    Task SaveChangesAsync();
}
