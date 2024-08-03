using Library.Model.Enums;
using Library.Model.Models;
using Library.Service.Dtos.Book.Get;
using System;

namespace Library.Service.Helpers.Books;

public static class BookMappingHelpers
{
    // create helpers
    public static void AddAuthorsToBook(this Book book, Guid[] authorIds)
    {
        foreach (var authorId in authorIds)
        {
            book.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = authorId });
        }
    }

    // edit helpers
    public static void UpdatePublisher(this Book book, Guid? newPublisherId)
    {
        book.PublisherId = newPublisherId;
    }

    /// <summary>
    /// Efficiently calculates the differences between the old and new locations,
    /// determining which locations were added, removed, or had their quantities changed.
    /// It does not perform any actual database operations, but instead returns the necessary
    /// information to update the book's locations.
    /// </summary>
    /// <param name="book">The book for which locations are being updated.</param>
    /// <param name="oldLocations">An array of BookLocationDto representing the current locations of the book.</param>
    /// <param name="newLocations">An array of BookLocationDto representing the desired new locations of the book.</param>
    /// <returns>
    /// A 3-element tuple containing two lists of BookLocationDto and a number by which total quantity was changed:
    /// - Item1: List of locations from which books need to be removed.
    /// - Item2: List of locations to which books need to be added.
    /// </returns>
    public static (List<BookLocationDto>, List<BookLocationDto>, int) UpdateLocations(this Book book, BookLocationDto[] oldLocations, BookLocationDto[] newLocations)
    {
        var locationChanges = new Dictionary<(int, int?, BookCopyStatus), int>(); // key = (room, shelf, status), value = quantity change
        foreach (var location in oldLocations)
        {
            UpdateLocationChanges(locationChanges, location, -1);
        }
        foreach (var location in newLocations)
        {
            UpdateLocationChanges(locationChanges, location, 1);
        }
        var locationsToRemove = new List<BookLocationDto>();
        var locationsToAdd = new List<BookLocationDto>();
        int count = 0;
        foreach (var changeKVP in locationChanges)
        {
            var ((room, shelf, status), change) = changeKVP;
            count += change; // change is negative when books are removed, so + is good for both cases
            if (change < 0)
            {
                locationsToRemove.Add(new BookLocationDto(room, shelf, status, -change));
            }
            else if (change > 0)
            {
                locationsToAdd.Add(new BookLocationDto(room, shelf, status, change));
            }
        }
        return (locationsToRemove, locationsToAdd, count);
    }

    private static void UpdateLocationChanges(Dictionary<(int, int?, BookCopyStatus), int> locationChanges, BookLocationDto location, int multiplier)
    {
        var key = (location.RoomId, location.ShelfId, location.Status);
        if (!locationChanges.ContainsKey(key))
        {
            locationChanges[key] = location.Quantity * multiplier;
        }
        else
        {
            locationChanges[key] += location.Quantity * multiplier;
        }
    }
}
