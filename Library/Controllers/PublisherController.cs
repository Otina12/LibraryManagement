using AutoMapper;
using Library.Service.Interfaces;
using Library.ViewModels.Publishers;
using Microsoft.AspNetCore.Mvc;
using Library.ViewSpecifications;
using Library.Service.Dtos;
using Library.Service.Dtos.Publisher.Get;
using Library.Service.Dtos.Publisher.Post;

namespace Library.Controllers;

public class PublisherController : BaseController
{
    public PublisherController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, bool includeDeleted, int pageNumber = 1, int pageSize = 10)
    {
        var publisherParams = new EntityFiltersDto<PublisherDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize,
            IncludeDeleted = includeDeleted
        };

        var publishers = await _serviceManager.PublisherService.GetAllFilteredPublishers(publisherParams);
        var publishersTable = IndexTables.GetPublisherTable(publishers);
        return View(publishersTable);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var publisherResult = await _serviceManager.PublisherService.GetPublisherById(id);

        if (publisherResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var publisherVM = _mapper.Map<PublisherViewModel>(publisherResult.Value());

        return View(publisherVM);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CreatePublisherViewModel();
        return PartialView("_CreatePartial", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePublisherViewModel publisherVM)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_CreatePartial", publisherVM);
        }

        var publisherDto = _mapper.Map<CreatePublisherDto>(publisherVM);
        var result = await _serviceManager.PublisherService.Create(publisherDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Publisher '{publisherVM.Name}' has been created");
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<ActionResult> Edit(Guid id)
    {
        var publisherResult = await _serviceManager.PublisherService.GetPublisherById(id);

        if (publisherResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var editViewModel = _mapper.Map<PublisherViewModel>(publisherResult.Value());

        return PartialView("_EditPartial", editViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PublisherViewModel publisherVM)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_EditPartial", publisherVM);
        }

        var publisherDto = _mapper.Map<PublisherDto>(publisherVM);
        var result = await _serviceManager.PublisherService.Update(publisherDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Publisher '{publisherVM.Name}' has been updated");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _serviceManager.PublisherService.Deactivate(id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Publisher has been deactivated successfully");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Renew(Guid id)
    {
        var result = await _serviceManager.PublisherService.Reactivate(id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Publisher has been reactivated successfully");
        return Json(new { success = true });
    }
}
