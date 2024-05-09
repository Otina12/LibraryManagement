using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Attributes;

public class RangeYearToCurrent : RangeAttribute
{
    public RangeYearToCurrent(int from) : base(from, DateTime.Today.Year)
    {
    }
}
