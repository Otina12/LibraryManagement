namespace Library.Model.Abstractions.Errors;

public class PublisherErrors
{
    public static readonly Error PublisherNotFound = new("Publisher.PublisherNotFound", "Publisher does not exist.");
    public static readonly Error PublisherAlreadyExists = new("Publisher.PublisherAlreadyExists", "Publisher already exists.");
}
