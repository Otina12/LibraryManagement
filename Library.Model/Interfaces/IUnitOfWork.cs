namespace Library.Model.Interfaces;

public interface IUnitOfWork // this will help use repositories and update made changes as 1 big chunk
{
    public IEmployeeRepository Employees { get; }
    public IRoleMenuRepository RoleMenus { get; }
    public IEmailRepository EmailTemplates { get; }
    public IPublisherRepository Publishers { get; }
    public IAuthorRepository Authors { get; }
    public IBookRepository Books { get; }

    Task SaveChangesAsync();
}
