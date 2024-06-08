using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class BaseModel
{
    public DateTime CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }

    [NotMapped]
    public bool IsDeleted => DeleteDate is not null;
}
