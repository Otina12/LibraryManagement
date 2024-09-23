using Library.Service.Interfaces;
using Library.Service.Dtos.Report;
using AutoMapper;
using Library.ViewModels.Reports;
using Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Library.Model.Enums;
using Microsoft.Reporting.NETCore;

namespace Library.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
        {
        }

        [HttpGet]
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

        public async Task<FileResult> ExportExcelPopularityReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model}{ReportType}{StartDate}-{EndDate}
        {
            var report = await _serviceManager.ReportService.GetPopularityReport(new PopularityReportDto(modelName, startDate, endDate));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Popularity Report {startDate.Date:yyyy-MM-dd} - {endDate.Date:yyyy-MM-dd}");
        }

        public async Task<FileResult> ExportPdfPopularityReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model}{ReportType}{StartDate}-{EndDate}
        {
            var report = await _serviceManager.ReportService.GetPopularityReport(new PopularityReportDto(modelName, startDate, endDate));
            var parameters = new ReportParameter[]
            {
                new("StartDate", startDate.ToString("dd-MM-yyyy")),
                new("EndDate", endDate.ToString("dd-MM-yyyy")),
                //new("ChartTitle", $"{modelName} Popularity Report {startDate:yyyy/MM/dd}-{endDate:yyyy/MM/dd}")
            };

            var reportBytes = PdfHelper.ExportToPDF(report, "Popularity", "Chart", parameters);
            return File(reportBytes, "application/pdf", $"PopularityReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }

        private async Task<IActionResult> GenerateAnnualReport(string model, int year)
        {
            var reportDto = new AnnualReportDto(model, year);
            var report = await _serviceManager.ReportService.GetAnnualReport(reportDto);

            var reportForViewDto = new AnnualReportViewDto(model, report, year);
            return View("AnnualReport", reportForViewDto);
        }

        public async Task<FileResult> ExportExcelAnnualReport(string modelName, int year) // filename is {Model}{ReportType}{Year}
        {
            var report = await _serviceManager.ReportService.GetAnnualReport(new AnnualReportDto(modelName, year));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Annual Report {year}");
        }

        public async Task<FileResult> ExportPdfAnnualReport(string modelName, int year) // filename is {Model}{ReportType}{StartDate}-{EndDate}
        {
            var report = await _serviceManager.ReportService.GetAnnualReport(new AnnualReportDto(modelName, year));
            var parameters = new ReportParameter[]
            {
                new("Year", year.ToString()),
                //new("ChartTitle", $"{modelName} Annual Report {year}")
            };

            var reportBytes = PdfHelper.ExportToPDF(report, "Annual", "Chart", parameters);
            return File(reportBytes, "application/pdf", $"AnnualReport_{year}.pdf");
        }

        private async Task<IActionResult> GenerateBooksDamagedReport(string model, DateTime startDate, DateTime endDate)
        {
            var reportDto = new BooksDamagedReportDto(model, startDate, endDate);
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(reportDto);

            var reportForViewDto = new BooksDamagedReportViewDto(model, report, startDate, endDate);
            return View("BooksDamagedReport", reportForViewDto);
        }

        public async Task<FileResult> ExportExcelBooksDamagedReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model}{ReportType}{StartDate}-{EndDate}
        {
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(new BooksDamagedReportDto(modelName, startDate, endDate));
            return ExcelHelper.ExportToExcel(report, $"{modelName} Books Damaged Report {startDate.Date:yyyy-MM-dd} - {endDate.Date:yyyy-MM-dd}");
        }

        public async Task<FileResult> ExportPdfBooksDamagedReport(string modelName, DateTime startDate, DateTime endDate) // filename is {Model}{ReportType}{StartDate}-{EndDate}
        {
            var report = await _serviceManager.ReportService.GetBooksDamagedReport(new BooksDamagedReportDto(modelName, startDate, endDate));
            var parameters = new ReportParameter[]
            {
                new("StartDate", startDate.ToString("dd-MM-yyyy")),
                new("EndDate", endDate.ToString("dd-MM-yyyy")),
                //new("ChartTitle", $"{modelName} Books Damaged Report {startDate:yyyyMMdd}_{endDate:yyyyMMdd}")
            };

            var reportBytes = PdfHelper.ExportToPDF(report, "BooksDamaged", "Chart", parameters);
            return File(reportBytes, "application/pdf", $"BooksDamagedReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }

        public async Task<FileResult> ExportPdfGeneralPopularityReport(DateTime startDate, DateTime endDate)
        {
            var bytes = await GeneratePDFs(startDate, endDate);
            return File(bytes, "application/pdf", $"GeneralReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }


        private async Task<byte[]> GeneratePDFs(DateTime startDate, DateTime endDate)
        {
            var mainPagePdf = PdfHelper.GenerateGeneralPage(await GenerateDataForReport(startDate, endDate), startDate, endDate);
            return mainPagePdf;
        }

        private async Task<IEnumerable<(string, IEnumerable<object>)>> GenerateDataForReport(DateTime startDate, DateTime endDate)
        {
            string[] models = ["Author", "Publisher", "Book", "OriginalBook", "Genre", "Employee", "Customer"];
            var data = new List<(string, IEnumerable<object>)>();

            foreach (var model in models)
            {
                var popularityReportData = await _serviceManager.ReportService.GetPopularityReport(new PopularityReportDto(model, startDate, endDate));
                data.Add(($"{model}Popularity", popularityReportData));

                if (model == "Book" || model == "Customer")
                {
                    var booksDamagedReportData = await _serviceManager.ReportService.GetBooksDamagedReport(new BooksDamagedReportDto(model, startDate, endDate));
                    data.Add(($"{model}BooksDamaged", booksDamagedReportData));
                }
            }

            return data;
        }
    }
}
