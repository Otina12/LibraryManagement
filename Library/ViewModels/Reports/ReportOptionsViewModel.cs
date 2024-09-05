namespace Library.ViewModels.Reports;

public class ReportOptionsViewModel
{
    public string ModelName { get; set; }
    public ReportType ReportType { get; set; }
    public int? Year { get; set; } // in case of annual report
    public DateTime? StartDate { get; set; } // in case of popularity
    public DateTime? EndDate { get; set; } // in case of popularity
}

public enum ReportType // will probably be extended as more types are added
{
    Popularity,
    Annual,
    BooksDamaged
}

