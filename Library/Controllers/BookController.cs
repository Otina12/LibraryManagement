using AutoMapper;
using Library.Service.Dtos.Book;
using Library.Service.Interfaces;
using Library.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Library.ViewSpecifications;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, int pageNumber = 1, int pageSize = 10)
    {
        var booksParams = new EntityFiltersDto<BookDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var books = await _serviceManager.BookService.GetAllFilteredBooks(booksParams);
        var booksTable = IndexTables.GetBookTable(books);
        return View(booksTable);
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
    public async Task<IActionResult> Edit(Guid id)
    {
        var bookDtoResult = await _serviceManager.BookService.GetBookById(id);

        if (bookDtoResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var bookVM = _mapper.Map<EditBookViewModel>(bookDtoResult.Value());

        await InitializeViewDropdowns();
        return View(bookVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBookViewModel editBookViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(editBookViewModel);
        }

        var editBookDto = _mapper.Map<EditBookDto>(editBookViewModel);
        var editBookResult = await _serviceManager.BookService.UpdateBook(editBookDto);

        return HandleResult(editBookResult, editBookViewModel, "The book has been updated successfully", editBookResult.Error.Message, "Book");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _serviceManager.BookService.Deactivate(id);
        return HandleResult(result, null, "The book has been deleted successfully", result.Error.Message, "Book");
    }

    public async Task<IActionResult> Renew(Guid id)
    {
        var result = await _serviceManager.BookService.Reactivate(id);
        return HandleResult(result, null, "The book has been renewed successfully", result.Error.Message, "Book");
    }

    // book create/edit essential dropdowns
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
