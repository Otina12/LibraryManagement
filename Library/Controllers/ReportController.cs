using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Service.Dtos.Report;
using AutoMapper;
using Library.ViewModels.Reports;
using Library.Model.Models.Report;
using Library.Extensions;
using DocumentFormat.OpenXml.Bibliography;

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

        private async Task<IActionResult> PopularityReport(PopularityReportViewModel popularityReportVM)
        {
            var reportDto = _mapper.Map<PopularityReportDto>(popularityReportVM);
            var report = await _serviceManager.ReportService.GetPopularityReport(reportDto);

            var reportForViewDto = new PopularityReportViewDto(popularityReportVM.ModelName, report, popularityReportVM.StartDate, popularityReportVM.EndDate);
            return View("PopularityReport", reportForViewDto);
        }

        public async Task<FileResult> ExportPopularityReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model} {ReportType} {StartDate} - {EndDate}
        {
            var report = await _serviceManager.ReportService.GetPopularityReport(new PopularityReportDto(modelName, startDate, endDate));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Popularity Report {startDate.Date:yyyy-MM-dd} - {endDate.Date:yyyy-MM-dd}");
        }

        private async Task<IActionResult> GenerateAnnualReport(AnnualReportViewModel annualReportVM)
        {
            var reportDto = _mapper.Map<AnnualReportDto>(annualReportVM);
            var report = await _serviceManager.ReportService.GetAnnualReport(reportDto);

            var reportForViewDto = new AnnualReportViewDto(annualReportVM.ModelName, report, annualReportVM.Year);
            return View("AnnualReport", reportForViewDto);
        }

        public async Task<FileResult> ExportAnnualReport(string modelName, int year) // filename is {Model} {ReportType} {Year}
        {
            var report = await _serviceManager.ReportService.GetAnnualReport(new AnnualReportDto(modelName, year));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Annual Report {year}");
        }
    }
}
