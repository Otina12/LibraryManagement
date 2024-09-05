using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace Library.Extensions;

public static class ExcelHelper
{
    public static FileResult ExportToExcel<T>(IEnumerable<T> items, string fileName)
    {
        var dataTable = ReportToDataTable(items);

        using var wb = new XLWorkbook();
        var worksheet = wb.Worksheets.Add("Sheet1");

        var table = worksheet.Cell(1, 1).InsertTable(dataTable);

        var headerRow = table.HeadersRow();
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        table.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        table.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        worksheet.Columns().AdjustToContents();

        foreach (var column in worksheet.Columns())
        {
            if (column.Width > 50)
            {
                column.Width = 50;
            }
        }

        worksheet.Rows().AdjustToContents();

        using var stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = $"{fileName}.xlsx"
        };
    }

    private static DataTable ReportToDataTable<T>(IEnumerable<T> items)
    {
        var table = CreateDataTableForPropertiesOfType<T>();
        PropertyInfo[] piT = typeof(T).GetProperties();

        foreach (var item in items)
        {
            var dr = table.NewRow();

            for (int property = 0; property < table.Columns.Count; property++)
            {
                if (piT[property].CanRead)
                {
                    dr[property] = piT[property].GetValue(item, null);
                }
            }

            table.Rows.Add(dr);
        }

        return table;
    }

    private static DataTable CreateDataTableForPropertiesOfType<T>()
    {
        DataTable dt = new DataTable();
        PropertyInfo[] piT = typeof(T).GetProperties();

        foreach (PropertyInfo pi in piT)
        {
            Type propertyType = null;
            if (pi.PropertyType.IsGenericType)
            {
                propertyType = pi.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                propertyType = pi.PropertyType;
            }
            DataColumn dc = new DataColumn(pi.Name, propertyType);

            if (pi.CanRead)
            {
                dt.Columns.Add(dc);
            }
        }

        return dt;
    }
}
