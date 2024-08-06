using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class BookCopyLogRepository : GenericRepository<BookCopyLog>, IBookCopyLogRepository
{
    public BookCopyLogRepository(ApplicationDbContext context) : base(context)
    {
        dbSet.Include(x => x.BookCopy);
    }
}
