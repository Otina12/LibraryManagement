using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Service.Dtos.Report;
using AutoMapper;
using Library.ViewModels.Reports;
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
                CreateFailureNotification("Invalid input");
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
                    return await GeneratePopularityReport(popularityReportVM);

                case ReportType.Annual:
                    var annualReportVM = new AnnualReportViewModel
                    {
                        ModelName = reportOptions.ModelName,
                        Year = reportOptions.Year!.Value
                    };
                    return await GenerateAnnualReport(annualReportVM);

                case ReportType.BooksDamaged:
                    var booksDamagedReportVM = new BooksDamagedReportViewModel
                    {
                        ModelName = reportOptions.ModelName,
                        StartDate = reportOptions.StartDate!.Value,
                        EndDate = reportOptions.EndDate!.Value
                    };
                    return await GenerateBooksDamagedReport(booksDamagedReportVM);

                default:
                    ModelState.AddModelError("", "Unsupported report type.");
                    return View(reportOptions);
            }
        }

        private async Task<IActionResult> GeneratePopularityReport(PopularityReportViewModel popularityReportVM)
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

        private async Task<IActionResult> GenerateBooksDamagedReport(BooksDamagedReportViewModel booksDamagedReportVM)
        {
            var reportDto = _mapper.Map<BooksDamagedReportDto>(booksDamagedReportVM);
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(reportDto);

            var reportForViewDto = new BooksDamagedReportViewDto(booksDamagedReportVM.ModelName, report, booksDamagedReportVM.StartDate, booksDamagedReportVM.EndDate);
            return View("BooksDamagedReport", reportForViewDto);
        }

        public async Task<FileResult> ExportBooksDamagedReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model} {ReportType} {StartDate} - {EndDate}
        {
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(new BooksDamagedReportDto(modelName, startDate, endDate));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Books Damaged Report {startDate.Date:yyyy-MM-dd} - {endDate.Date:yyyy-MM-dd}");
        }
    }
}
