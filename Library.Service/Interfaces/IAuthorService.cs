﻿using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;

namespace Library.Service.Interfaces;

public interface IAuthorService : IBaseService<Author>
{
    Task<EntityFiltersDto<AuthorDto>> GetAllFilteredAuthors(EntityFiltersDto<AuthorDto> authorFilters);
    Task<IOrderedEnumerable<AuthorIdAndNameDto>> GetAllAuthorIdAndNames();
    Task<Result<AuthorDto>> GetAuthorById(Guid id);
    Task<Result> Create(CreateAuthorDto authorDto);
    Task<Result> Update(AuthorDto authorDto);
}
