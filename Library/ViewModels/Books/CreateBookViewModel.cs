﻿using Library.Service.Dtos.Book.Get;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Books;

public class CreateBookViewModel
{
    [MaxLength(13, ErrorMessage = "ISBN must be at most 13 characters long")]
    public string ISBN { get; set; }
    public string Title { get; set; }
    public int Edition { get; set; }
    public int PageCount { get; set; }
    public string Description { get; set; }
    public int PublishYear { get; set; }

    [Display(Name = "Genres")]
    public List<int> SelectedGenreIds { get; set; } = [];

    [Display(Name = "Publisher")]
    public Guid? SelectedPublisherId { get; set; }

    [Display(Name = "Authors")]
    public List<Guid> SelectedAuthorIds { get; set; } = [];

    public List<BookLocationDto> Locations { get; set; } = [];
}