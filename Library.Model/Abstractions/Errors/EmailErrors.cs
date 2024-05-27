namespace Library.Model.Abstractions.Errors;

public static class EmailErrors
{
    public static readonly Error EmailTemplateNotFound = new("Email.EmailTemplateNotFound", "Email template was not found");
}
