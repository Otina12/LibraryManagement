namespace Library.Model.Exceptions;

public class EmailTemplateAlreadyExistsException(string message) : Exception(message)
{
}