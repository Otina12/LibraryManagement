﻿using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Book.Post;

namespace Library.Service.Interfaces;

public interface IBookService : IBaseService<Book>
{
    IEnumerable<BookIdTitleAndQuantityDto> GetAllBooksSorted(bool includeDeleted = false);
    Task<EntityFiltersDto<BookDto>> GetAllFilteredBooks(EntityFiltersDto<BookDto> bookFilters);
    Task<Result<BookDetailsDto>> GetBookById(Guid id);
    Task<Result> CreateBook(CreateBookDto bookDto);
    Task<Result> UpdateBook(EditBookDto bookDto);
}
