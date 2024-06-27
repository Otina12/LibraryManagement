using AutoMapper;
using Library.Model.Enums;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.Service.Interfaces;
using Library.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, int pageNumber = 1, int pageSize = 10)
    {
        var booksParams = new BookListDto
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var books = await _serviceManager.BookService.GetAllFilteredBooks(booksParams);
        return View(books);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var book = await _serviceManager.BookService.GetBookById(id);
        return View(book.Value());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await InitializeViewDropdowns();
        return View(new CreateBookViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookViewModel createBookViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(createBookViewModel);
        }

        var createBookDto = _mapper.Map<CreateBookDto>(createBookViewModel);
        var createBookResult = await _serviceManager.BookService.CreateBook(createBookDto);

        return HandleResult(createBookResult, createBookViewModel, "The book copies have been added successfully", createBookResult.Error.Message, "Book", "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        await InitializeViewDropdowns();
        return View(new EditBookViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBookViewModel editBookViewModel)
    {
        throw new NotImplementedException();
    }


    private async Task InitializeViewDropdowns()
    {
        ViewBag.Publishers = await _serviceManager.PublisherService.GetAllPublisherIdAndNames();
        ViewBag.Authors = await _serviceManager.AuthorService.GetAllAuthorIdAndNames();
        ViewBag.Genres = await _serviceManager.GenreService.GetAllGenres();

        var roomShelfDictionary = await _serviceManager.ShelfService.GetRoomShelves();
        ViewBag.Rooms = roomShelfDictionary.Keys;
        ViewBag.Shelves = JsonSerializer.Serialize(roomShelfDictionary);
    }
}
