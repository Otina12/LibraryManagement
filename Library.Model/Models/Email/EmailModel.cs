namespace Library.Model.Models.Email;

public class EmailModel
{
    public Guid Id { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
