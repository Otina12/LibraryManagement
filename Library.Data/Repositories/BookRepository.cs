namespace Library.Data.Repositories;

using Library.Model.Interfaces;
using Library.Model.Models;

internal class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }
}
