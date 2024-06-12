using AutoMapper;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.Service.Interfaces;
using Library.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index()
    {
        var books = await _serviceManager.BookService.GetAllBooks();
        return View(books);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var publishers = (await _serviceManager.PublisherService.GetAllPublishers())
            .Select(x => new PublisherIdAndNameDto(x.Id, x.Name))
            .OrderBy(x => x.Name);

        var authors = (await _serviceManager.AuthorService.GetAllAuthors())
            .Select(x => new AuthorIdAndNameDto(x.Id, $"{x.Name} {x.Surname}"))
            .OrderBy(x => x.FullName);

        var viewModel = new CreateBookViewModel
        {
            Publishers = new SelectList(publishers, "Id", "Name"),
            Authors = new SelectList(authors, "Id", "FullName")
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookViewModel bookViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(bookViewModel);
        }

        var createBookDto = _mapper.Map<CreateBookDto>(bookViewModel);

        var createBookResult = await _serviceManager.BookService.CreateBook(createBookDto);

        if (createBookResult.IsFailure)
        {
            CreateFailureNotification(createBookResult.Error.Message);
        }

        CreateSuccessNotification("The book copies have been added succesfully");
        return RedirectToAction("Index", "Book");
    }

    
}
