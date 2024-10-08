﻿using AutoMapper;
using Library.Service.Dtos;
using Library.Service.Interfaces;
using Library.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Library.ViewSpecifications;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Book.Post;
using Library.Service.Dtos.BookCopy.Post;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, bool includeDeleted, int pageNumber = 1, int pageSize = 10)
    {
        var booksParams = new EntityFiltersDto<BookDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize,
            IncludeDeleted = includeDeleted
        };

        var books = await _serviceManager.BookService.GetAllFilteredBooks(booksParams);
        var booksTable = IndexTables.GetBookTable(books);
        return View(booksTable);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var bookResult = await _serviceManager.BookService.GetBookById(id);
        if (bookResult.IsFailure)
        {
            CreateFailureNotification($"Book with ID: '{id}' does not exist");
            return RedirectToAction("Index", "Book");
        }

        return View(bookResult.Value());
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
            await InitializeViewDropdowns();
            return View(createBookViewModel);
        }

        var createBookDto = _mapper.Map<CreateBookDto>(createBookViewModel);
        var createBookResult = await _serviceManager.BookService.CreateBook(createBookDto, string.Empty);

        if (createBookResult.IsFailure)
        {
            await InitializeViewDropdowns();
            CreateFailureNotification(createBookResult.Error.Message);
            return View(createBookViewModel);
        }

        CreateSuccessNotification("The book has been added successfully");
        return RedirectToAction("Index", "Book");
    }

    [HttpGet]
    public async Task<IActionResult> CreateCopies(string bookId)
    {
        var viewModel = new CreateBookCopiesViewModel();
        viewModel.BookId = bookId is null ? Guid.Empty : new Guid(bookId);
        await InitializeViewDropdowns();
        return PartialView("_CreateCopies", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCopies(CreateBookCopiesViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await InitializeViewDropdowns();
            return PartialView("_CreateCopies", viewModel);
        }

        var bookCopiesDto = _mapper.Map<CreateBookCopiesDto>(viewModel);
        var result = await _serviceManager.BookService.CreateBookCopies(bookCopiesDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Succesfully added book copies");
        return Json(new { success = true });
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
        var editBookResult = await _serviceManager.BookService.UpdateBook(editBookDto, string.Empty);

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
        var booksDictionary = await _serviceManager.BookService.GetAllBookEditions(false, currentCulture);

        var serializedDictionary = booksDictionary.ToDictionary(
            kvp => kvp.Key.Id.ToString(),
            kvp => new
            {
                Title = kvp.Key.Title,
                Editions = kvp.Value
            }
        );

        ViewBag.OriginalBooks = booksDictionary.Keys.Select(k => new { Id = k.Id, Title = k.Title }).ToList();
        ViewBag.BookEditions = JsonSerializer.Serialize(serializedDictionary);
        ViewBag.Publishers = await _serviceManager.PublisherService.GetAllPublisherIdAndNames();
        ViewBag.Authors = await _serviceManager.AuthorService.GetAllAuthorIdAndNames();

        var roomShelfDictionary = await _serviceManager.ShelfService.GetRoomShelves();
        ViewBag.Rooms = roomShelfDictionary.Keys;
        ViewBag.Shelves = JsonSerializer.Serialize(roomShelfDictionary);
    }
}
