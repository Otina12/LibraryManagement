using Library.Model.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Library.Model.Interfaces;

public interface IUnitOfWork // this will help use repositories and update made changes as 1 big chunk
{
    public IEmployeeRepository Employees { get; }
    public IRoleMenuRepository RoleMenus { get; }
    public IEmailRepository EmailTemplates { get; }
    public IPublisherRepository Publishers { get; }
    public IAuthorRepository Authors { get; }
    public IBookRepository Books { get; }
    public IBookCopyRepository BookCopies { get; }
    public IGenreRepository Genres { get; }
    public IRoomRepository Rooms { get; }
    public IShelfRepository Shelves { get; }
    public IBaseModelRepository<T> GetBaseModelRepository<T>() where T : BaseModel;

    Task SaveChangesAsync();
    EntityEntry<T> Entry<T>(T entity) where T : class;
}
