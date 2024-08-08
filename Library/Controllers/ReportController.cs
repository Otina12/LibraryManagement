using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Service.Dtos.Report;
using AutoMapper;
using Library.ViewModels.Reports;
using Library.Model.Models.Report;
using Library.ViewSpecifications;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Library.ViewModels.Shared;

namespace Library.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePopularityReport(PopularityReportViewModel reportVM)
        {
            var valResult = Validate(reportVM);
            if (valResult.IsFailure)
            {
                return View(reportVM);
            }

            var reportDto = _mapper.Map<PopularityReportDto>(reportVM);
            var result = await _serviceManager.ReportService.GetPopularityReport(reportDto);
            var reportTable = IndexTables.GetPopularityTable(new Service.Dtos.EntityFiltersDto<PopularityReportRow>()
            {
                Entities = result
            });

            return View("PopularityReport", reportTable);
        }

        public async Task<IActionResult> ExportToExcel(SortableTableModel model)
        {
            // implement later
            return Ok();
        }
    }
}
