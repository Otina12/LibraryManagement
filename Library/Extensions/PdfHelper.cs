using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

namespace Library.Extensions;

public static class PdfHelper
{
    private readonly static string _rdlcReportPath = "C:\\Users\\Giorgi\\source\\repos\\Library\\Library.Web\\Rdlc\\Reports";

    // Naming convention for each rdlc file is '{Model}{ReportType}{FormatType}', where:
    // Model is entity which we query on
    // ReportType is an enum that includes all types that we offer (popularity, annual, books damaged...)
    // FormatType is either chart of table (for now)

    public static byte[] ExportToPDF<T>(IEnumerable<T> items, string reportType, string formatType, ReportParameter[] parameters)
    {
        using var report = new LocalReport();
        report.ReportPath = Path.Combine(_rdlcReportPath, reportType, $"{reportType}Report{formatType}.rdlc");
        report.DataSources.Add(new ReportDataSource($"ds{reportType}Report", items));

        report.SetParameters(parameters);

        var bytes = report.Render("PDF");
        return bytes;
    }

    public static byte[] MergePdfs(List<byte[]> pdfs)
    {
        using var outputDocument = new PdfDocument();
        foreach (var pdf in pdfs)
        {
            using var memoryStream = new MemoryStream(pdf);
            using var inputDocument = PdfReader.Open(memoryStream, PdfDocumentOpenMode.Import);

            for (int i = 0; i < inputDocument.PageCount; i++)
            {
                outputDocument.AddPage(inputDocument.Pages[i]);
            }
        }

        using var outputStream = new MemoryStream();
        outputDocument.Save(outputStream, false);
        return outputStream.ToArray();
    }
}
