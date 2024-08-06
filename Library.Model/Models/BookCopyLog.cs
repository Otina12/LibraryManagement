using Library.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class BookCopyLog
{
    public Guid Id { get; set; }
    [ForeignKey("BookCopy")]
    public Guid BookCopyId { get; set; }
    public BookCopyAction BookCopyAction { get; set; }
    public BookCopyStatus CurrentStatus { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ActionTimeStamp { get; set; }
    public BookCopy BookCopy { get; }
}

public enum BookCopyAction
{
    Created,
    Reserved,
    Returned, // can be of any BookCopyStatus (except lost)
    Lost,
    Deleted
}
