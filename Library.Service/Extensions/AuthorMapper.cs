using Library.Model.Models;
using Library.Service.Dtos.Author;

namespace Library.Service.Extensions
{
    public static class AuthorMapper
    {
        public static AuthorDto MapToAuthorDto(this Author author)
        {
            return new AuthorDto(
                author.Id,
                author.Name,
                author.Surname,
                author.Email,
                author.Description,
                author.BirthYear,
                author.DeathYear,
                author.CreationDate
            );
        }

        public static Author MapToAuthor(this CreateAuthorDto authorDto)
        {
            return new Author()
            {
                Name = authorDto.Name,
                Surname = authorDto.Surname,
                Email = authorDto.Email,
                Description = authorDto.Description,
                BirthYear = authorDto.BirthYear,
                DeathYear = authorDto.DeathYear,
                CreationDate = DateTime.UtcNow
            };
        }

        public static Author MapToAuthor(this AuthorDto authorDto)
        {
            return new Author()
            {
                Id = authorDto.Id,
                Name = authorDto.Name,
                Surname = authorDto.Surname,
                Email = authorDto.Email,
                Description = authorDto.Description,
                BirthYear = authorDto.BirthYear,
                DeathYear = authorDto.DeathYear,
                CreationDate = authorDto.CreationDate
            };
        }
    }
}
