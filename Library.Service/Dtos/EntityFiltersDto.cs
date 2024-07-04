﻿namespace Library.Service.Dtos.Book;

// used to sort, search, filter generic collections
public record EntityFiltersDto<T> where T : class
{
    public IEnumerable<T> Entities { get; set; } = [];
    public string SearchString { get; set; } = string.Empty;
    public string SortBy { get; set; } = string.Empty;
    public string SortOrder { get; set; } = string.Empty;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
}