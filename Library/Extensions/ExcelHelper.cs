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

        using XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(dataTable);

        using MemoryStream stream = new MemoryStream();
        wb.SaveAs(stream);

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
