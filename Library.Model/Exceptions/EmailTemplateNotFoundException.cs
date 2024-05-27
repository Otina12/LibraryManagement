namespace Library.Model.Exceptions;

public class EmailTemplateNotFoundException(string message) : Exception(message)
{
}