using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class BookCopyRepository : BaseModelRepository<BookCopy>, IBookCopyRepository
{
    public BookCopyRepository(ApplicationDbContext context) : base(context)
    {
    }

}
