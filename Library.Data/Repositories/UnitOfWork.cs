using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Library.Data.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;

    public IEmployeeRepository Employees { get; private set; }
    public IRoleMenuRepository RoleMenus { get; private set; }
    public IEmailRepository EmailTemplates { get; private set; }
    public IPublisherRepository Publishers { get; private set; }
    public IAuthorRepository Authors { get; private set; }
    public IBookRepository Books { get; private set; }
    public IBookCopyRepository BookCopies { get; private set; }
    public IGenreRepository Genres { get; private set; }
    public IRoomRepository Rooms { get; private set; }
    public IShelfRepository Shelves { get; private set; }
    public ICustomerRepository Customers { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Employees = new EmployeeRepository(_context);
        RoleMenus = new RoleMenuRepository(_context);
        EmailTemplates = new EmailRepository(_context);
        Publishers = new PublisherRepository(_context);
        Authors = new AuthorRepository(_context);
        Books = new BookRepository(_context);
        BookCopies = new BookCopyRepository(_context);
        Genres = new GenreRepository(_context);
        Rooms = new RoomRepository(_context);
        Shelves = new ShelfRepository(_context);
        Customers = new CustomerRepository(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
        return _context.Entry(entity);
    }

    public void Detach<T>(T entity) where T : class
    {
        _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
    }

    public IBaseModelRepository<T> GetBaseModelRepository<T>() where T : BaseModel
    {
        return new BaseModelRepository<T>(_context);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}
