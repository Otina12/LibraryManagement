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
                    return await GeneratePopularityReport(reportOptions.ModelName, reportOptions.StartDate!.Value, reportOptions.EndDate!.Value);

                case ReportType.Annual:
                    return await GenerateAnnualReport(reportOptions.ModelName, reportOptions.Year!.Value);

                case ReportType.BooksDamaged:
                    return await GenerateBooksDamagedReport(reportOptions.ModelName, reportOptions.StartDate!.Value, reportOptions.EndDate!.Value);

                default:
                    ModelState.AddModelError("", "Unsupported report type.");
                    return View(reportOptions);
            }
        }

        private async Task<IActionResult> GeneratePopularityReport(string model, DateTime startDate, DateTime endDate)
        {
            var reportDto = new PopularityReportDto(model, startDate, endDate);
            var report = await _serviceManager.ReportService.GetPopularityReport(reportDto);

            var reportForViewDto = new PopularityReportViewDto(model, report, startDate, endDate);
            return View("PopularityReport", reportForViewDto);
        }

        public async Task<FileResult> ExportPopularityReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model} {ReportType} {StartDate} - {EndDate}
        {
            var report = await _serviceManager.ReportService.GetPopularityReport(new PopularityReportDto(modelName, startDate, endDate));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Popularity Report {startDate.Date:yyyy-MM-dd} - {endDate.Date:yyyy-MM-dd}");
        }

        private async Task<IActionResult> GenerateAnnualReport(string model, int year)
        {
            var reportDto = new AnnualReportDto(model, year);
            var report = await _serviceManager.ReportService.GetAnnualReport(reportDto);

            var reportForViewDto = new AnnualReportViewDto(model, report, year);
            return View("AnnualReport", reportForViewDto);
        }

        public async Task<FileResult> ExportAnnualReport(string modelName, int year) // filename is {Model} {ReportType} {Year}
        {
            var report = await _serviceManager.ReportService.GetAnnualReport(new AnnualReportDto(modelName, year));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Annual Report {year}");
        }

        private async Task<IActionResult> GenerateBooksDamagedReport(string model, DateTime startDate, DateTime endDate)
        {
            var reportDto = new BooksDamagedReportDto(model, startDate, endDate);
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(reportDto);

            var reportForViewDto = new BooksDamagedReportViewDto(model, report, startDate, endDate);
            return View("BooksDamagedReport", reportForViewDto);
        }

        public async Task<FileResult> ExportBooksDamagedReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model} {ReportType} {StartDate} - {EndDate}
        {
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(new BooksDamagedReportDto(modelName, startDate, endDate));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Books Damaged Report {startDate.Date:yyyy-MM-dd} - {endDate.Date:yyyy-MM-dd}");
        }
    }
}
