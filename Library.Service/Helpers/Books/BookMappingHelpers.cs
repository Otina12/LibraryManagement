using Library.Model.Models;
using Library.Service.Dtos.Book;
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

    public static void AddGenresToBook(this Book book, int[] genreIds)
    {
        foreach (var genreId in genreIds)
        {
            book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genreId });
        }
    }

    // edit helpers
    public static void UpdateGenres(this Book book, List<int> oldGenreIds, List<int> newGenreIds)
    {
        var genresToBeDeleted = oldGenreIds.Except(newGenreIds);
        var genresToBeAdded = newGenreIds.Except(oldGenreIds);

        foreach(var genreId in genresToBeDeleted)
        {
            book.BookGenres.Remove(new BookGenre { BookId = book.Id, GenreId = genreId });
        }

        book.AddGenresToBook(genresToBeAdded.ToArray());
    }

    public static void UpdateAuthors(this Book book, List<Guid> oldAuthorIds, List<Guid> newAuthorIds)
    {
        var authorsToBeDeleted = oldAuthorIds.Except(newAuthorIds);
        var authorsToBeAdded = newAuthorIds.Except(oldAuthorIds);

        foreach (var authorId in authorsToBeDeleted)
        {
            book.BookAuthors.Remove(new BookAuthor { BookId = book.Id, AuthorId = authorId });
        }

        book.AddAuthorsToBook(authorsToBeAdded.ToArray());
    }

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
        var locationChanges = new Dictionary<(int, int?), int>();

        foreach(var location in oldLocations)
        {
            UpdateLocationChanges(locationChanges, location, -1);
        }

        foreach(var location in newLocations)
        {
            UpdateLocationChanges(locationChanges, location, 1);
        }

        var locationsToRemove = new List<BookLocationDto>();
        var locationsToAdd = new List<BookLocationDto>();
        int count = 0;

        foreach (var changeKVP in locationChanges)
        {
            var ((room, shelf), change) = changeKVP;
            count += change; // change is negative when books are removed, so + is good for both cases

            if (change < 0)
            {
                locationsToRemove.Add(new BookLocationDto(room, shelf, -change));
            }
            else if (change > 0)
            {
                locationsToAdd.Add(new BookLocationDto(room, shelf, change));
            }
        }

        return (locationsToRemove, locationsToAdd, count);
    }

    private static void UpdateLocationChanges(Dictionary<(int, int?), int> locationChanges, BookLocationDto location, int multiplier)
    {
        var key = (location.RoomId, location.ShelfId);

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
