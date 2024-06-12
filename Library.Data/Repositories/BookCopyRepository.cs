using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class BookCopyRepository : GenericRepository<BookCopy>, IBookCopyRepository
{
    public BookCopyRepository(ApplicationDbContext context) : base(context)
    {
    }

}
