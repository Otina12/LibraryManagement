using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Service.Dtos.Report;
using AutoMapper;
using Library.ViewModels.Reports;
using Library.Model.Models.Report;
using Library.Extensions;

namespace Library.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController(IServiceManager serviceManager, IMapper mapper)
            : base(serviceManager, mapper)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReportOptionsViewModel reportOptions)
        {
            var validationResult = Validate(reportOptions);
            if (validationResult.IsFailure)
            {
                return View(reportOptions);
            }

            switch (reportOptions.ReportType)
            {
                case ReportType.Popularity:
                    var popularityReportVM = new PopularityReportViewModel
                    {
                        ModelName = reportOptions.ModelName,
                        StartDate = reportOptions.StartDate!.Value,
                        EndDate = reportOptions.EndDate!.Value
                    };
                    return await PopularityReport(popularityReportVM);

                case ReportType.Annual:
                    var annualReportVM = new AnnualReportViewModel
                    {
                        ModelName = reportOptions.ModelName,
                        Year = reportOptions.Year!.Value
                    };
                    return await GenerateAnnualReport(annualReportVM);

                default:
                    ModelState.AddModelError("", "Unsupported report type.");
                    return View(reportOptions);
            }
        }

        private async Task<IActionResult> PopularityReport(PopularityReportViewModel reportVM)
        {
            var reportDto = _mapper.Map<PopularityReportDto>(reportVM);
            var report = await _serviceManager.ReportService.GetPopularityReport(reportDto);

            var reportForViewDto = new PopularityReportViewDto(reportVM.ModelName, report, reportVM.StartDate, reportVM.EndDate);
            return View("PopularityReport", reportForViewDto);
        }

        [HttpPost]
        public FileResult ExportPopularityReport(string fileName, IEnumerable<PopularityReportRow> report) // filename is {Model}{ReportType} + if annual: {Year}, else if popularity: {StartDate}-{EndDate}
        {
            //var fileName = $"{popularityReport.ModelName} Popularity {popularityReport.StartDate:yyyy-MM-dd} - {popularityReport.EndDate:yyyy-MM-dd}.xlsx";
            return ExcelHelper.ExportToExcel(report, fileName);
        }

        private async Task<IActionResult> GenerateAnnualReport(AnnualReportViewModel annualReportVM)
        {
            var reportDto = _mapper.Map<AnnualReportDto>(annualReportVM);
            var report = await _serviceManager.ReportService.GetAnnualReport(reportDto);

            return View(report);
        }
    }
}
