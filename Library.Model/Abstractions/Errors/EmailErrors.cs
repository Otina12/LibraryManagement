namespace Library.Model.Abstractions.Errors;

public static class EmailErrors
{
    public static readonly Error EmailTemplateNotFound = new("Email.EmailTemplateNotFound", "Email template was not found");
    public static readonly Error EmailTemplateAlreadyExists = new("Email.EmailTemplateAlreadyExists", "Email template already exists");
    public static readonly Error InvalidModel = new("Email.InvalidModel", "Model does not contain needed properties");
}
