using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using DocumentFormat.OpenXml.Spreadsheet;
using Library.Model.Enums;

namespace Library.Extensions;

public static class PdfHelper
{
    private readonly static string _rdlcReportPath = "C:\\Users\\Giorgi\\source\\repos\\Library\\Library.Web\\Rdlc\\Reports";

    // Naming convention for each rdlc file is '{Model}{ReportType}{FormatType}', where:
    // Model is the entity which we need.
    // ReportType is an enum that includes all types that we offer (popularity, annual, books damaged...).
    // FormatType is either chart of table (for now).

    public static byte[] ExportToPDF<T>(IEnumerable<T> items, string reportType, string formatType, ReportParameter[] parameters)
    {
        using var report = new LocalReport();
        report.ReportPath = Path.Combine(_rdlcReportPath, reportType, $"{reportType}Report{formatType}.rdlc");
        report.DataSources.Add(new ReportDataSource($"ds{reportType}Report", items));
        report.SetParameters(parameters);

        var bytes = report.Render("PDF");
        return bytes;
    }

    public static byte[] GenerateGeneralPage<T>(IEnumerable<(string, IEnumerable<T>)> data, DateTime startDate, DateTime endDate)
    {
        var parameters = new ReportParameter[]
        {
            new("startDate", startDate.ToString()),
            new("endDate", endDate.ToString())
        };

        using var report = new LocalReport();

        report.ReportPath = Path.Combine(_rdlcReportPath, "GeneralReport.rdlc");

        foreach(var (dataSourceName, items) in data)
        {
            report.DataSources.Add(new ReportDataSource(dataSourceName, items));
        }
        
        report.SetParameters(parameters);

        var bytes = report.Render("PDF");
        return bytes;
    }
}
