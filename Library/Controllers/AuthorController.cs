using AutoMapper;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Interfaces;
using Library.ViewModels.Authors;
using Library.ViewSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class AuthorController : BaseController
{
    public AuthorController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, bool includeDeleted, int pageNumber = 1, int pageSize = 10)
    {
        var authorParams = new EntityFiltersDto<AuthorDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize,
            IncludeDeleted = includeDeleted
        };

        var authors = await _serviceManager.AuthorService.GetAllFilteredAuthors(authorParams);
        var authorsTable = IndexTables.GetAuthorTable(authors);
        return View(authorsTable);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var authorResult = await _serviceManager.AuthorService.GetAuthorById(id);

        if (authorResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var authorVM = _mapper.Map<AuthorViewModel>(authorResult.Value());

        return View(authorVM);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CreateAuthorViewModel();
        return PartialView("_CreatePartial", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAuthorViewModel authorVM)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_CreatePartial", authorVM);
        }

        var authorDto = _mapper.Map<CreateAuthorDto>(authorVM);
        var result = await _serviceManager.AuthorService.Create(authorDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Author {authorVM.Name} has been created");
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<ActionResult> Edit(Guid id)
    {
        var authorResult = await _serviceManager.AuthorService.GetAuthorById(id);

        if (authorResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var editViewModel = _mapper.Map<AuthorViewModel>(authorResult.Value());

        return PartialView("_EditPartial", editViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AuthorViewModel authorVM)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_CreatePartial", authorVM);
        }

        var authorDto = _mapper.Map<AuthorDto>(authorVM);
        var result = await _serviceManager.AuthorService.Update(authorDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Author {authorVM.Name} has been updated");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _serviceManager.AuthorService.Deactivate(id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Author has been deactivated successfully");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Renew(Guid id)
    {
        var result = await _serviceManager.AuthorService.Reactivate(id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Author has been reactivated successfully");
        return Json(new { success = true });
    }
}
