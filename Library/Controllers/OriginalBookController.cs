using AutoMapper;
using Library.Service.Dtos;
using Library.Service.Interfaces;
using Library.ViewSpecifications;
using Microsoft.AspNetCore.Mvc;
using Library.Service.Dtos.OriginalBook.Get;
using Library.ViewModels.OriginalBooks;
using Library.Service.Dtos.OriginalBook.Post;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Library.Controllers;

public class OriginalBookController : BaseController
{
    public OriginalBookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, bool includeDeleted, int pageNumber = 1, int pageSize = 10)
    {
        var originalBooksParams = new EntityFiltersDto<OriginalBookDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize,
            IncludeDeleted = includeDeleted
        };

        var originalBooks = await _serviceManager.OriginalBookService.GetAllFilteredOriginalBooks(originalBooksParams, currentCulture);
        var booksTable = IndexTables.GetOriginalBookTable(originalBooks);
        return View(booksTable);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var originalBookResult = await _serviceManager.OriginalBookService.GetOriginalBookById(id, currentCulture);
        if (originalBookResult.IsFailure)
        {
            CreateFailureNotification($"Original Book with ID: '{id}' does not exist");
            return RedirectToAction("Index", "OriginalBook");
        }

        return View(originalBookResult.Value());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateOriginalBookViewModel();
        ViewBag.Genres = await _serviceManager.GenreService.GetAllGenres();

        return PartialView("_Create", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOriginalBookViewModel createOriginalBookVM)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Genres = await _serviceManager.GenreService.GetAllGenres();
            return PartialView("_Create", createOriginalBookVM);
        }

        var createOriginalBookDto = _mapper.Map<CreateOriginalBookDto>(createOriginalBookVM);
        var result = await _serviceManager.OriginalBookService.Create(createOriginalBookDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Book '{createOriginalBookVM.Title}' has been created");
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<ActionResult> Edit(Guid id)
    {
        var originalBookResult = await _serviceManager.OriginalBookService.GetOriginalBookForEditById(id);

        if (originalBookResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var editViewModel = _mapper.Map<EditOriginalBookViewModel>(originalBookResult.Value());
        ViewBag.Genres = await _serviceManager.GenreService.GetAllGenres();

        return PartialView("_Edit", editViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditOriginalBookViewModel originalBookVM)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Genres = await _serviceManager.GenreService.GetAllGenres();
            return PartialView("_Edit", originalBookVM);
        }

        var originalBookDto = _mapper.Map<OriginalBookDto>(originalBookVM);
        var result = await _serviceManager.OriginalBookService.Update(originalBookDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Book has been updated");
        return Json(new { success = true });
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _serviceManager.OriginalBookService.Deactivate(id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Book has been deactivated successfully");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Renew(Guid id)
    {
        var result = await _serviceManager.OriginalBookService.Reactivate(id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Book has been reactivated successfully");
        return Json(new { success = true });
    }

}