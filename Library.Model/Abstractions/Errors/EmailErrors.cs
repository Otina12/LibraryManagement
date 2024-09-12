namespace Library.Model.Abstractions.Errors;

public static class EmailErrors
{
    public static readonly Error InvalidModel = new("Email.InvalidModel", "Model does not contain needed properties");
}
