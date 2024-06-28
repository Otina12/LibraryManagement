using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Service.Helpers.Books
{
    public static class BookQueryHelpers
    {
        public static IQueryable<Book> ApplySort(this IQueryable<Book> books, string sortBy, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortOrder))
            {
                return books;
            }
            return (sortBy.ToLower(), sortOrder) switch
            {
                ("title", "asc") => books.OrderBy(b => b.Title),
                ("title", "desc") => books.OrderByDescending(b => b.Title),
                ("year", "asc") => books.OrderBy(b => b.PublishYear),
                ("year", "desc") => books.OrderByDescending(b => b.PublishYear),
                ("quantity", "asc") => books.OrderBy(b => b.Quantity),
                ("quantity", "desc") => books.OrderByDescending(b => b.Quantity),
                _ => books
            };
        }

        public static IQueryable<Book> ApplyPagination(this IQueryable<Book> books, int pageNumber, int pageSize)
        {
            return books.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<Book> ApplySearch(this IQueryable<Book> books, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                string searchLower = searchString.ToLower();
                return books.Where(b => EF.Functions.Like(b.Title.ToLower(), $"%{searchLower}%") ||
                                        EF.Functions.Like(b.ISBN.ToLower(), $"%{searchLower}%"));
            }
            return books;
        }
    }
}
