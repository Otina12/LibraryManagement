using AutoMapper;
using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Model.Models.Menu;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Authorization;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Email;
using Library.Service.Dtos.Employee;
using Library.Service.Dtos.Publisher;
using Library.ViewModels;
using Library.ViewModels.Authorization;
using Library.ViewModels.Authors;
using Library.ViewModels.Books;
using Library.ViewModels.Emails;
using Library.ViewModels.Employees;
using Library.ViewModels.Publishers;

namespace Library.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // authorization maps
        CreateMap<RegisterViewModel, RegisterDto>();

        CreateMap<LoginViewModel, LoginDto>();

        CreateMap<ResetPasswordViewModel, ResetPasswordDto>();

        CreateMap<EmployeeRolesViewModel, EmployeeDto>();

        // email maps
        CreateMap<EditEmailTemplateViewModel, EditEmailDto>();
        CreateMap<EmailModel, EditEmailTemplateViewModel>();
        CreateMap<CreateEmailTemplateViewModel, CreateEmailDto>();

        // menu maps
        CreateMap<NavigationMenu, NavigationMenuItemViewModel>();

        // publisher maps
        CreateMap<PublisherDto, PublisherViewModel>().ReverseMap();
        CreateMap<CreatePublisherViewModel, CreatePublisherDto>();
        CreateMap<Publisher, PublisherDto>();

        // author maps
        CreateMap<AuthorDto, AuthorViewModel>().ReverseMap();
        CreateMap<CreateAuthorViewModel, CreateAuthorDto>();
        CreateMap<Author, AuthorDto>();


        // book maps
        CreateMap<CreateBookViewModel, CreateBookDto>();
        CreateMap<BookDetailsDto, EditBookViewModel>()
            .ForMember(dest => dest.AuthorIds, opt => opt.MapFrom(src => src.AuthorsDto.Select(a => a.Id).ToList()))
            .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.PublisherDto != null ? src.PublisherDto.Id : Guid.Empty))
            .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.Genres.Select(g => g.Id).ToList()));
        CreateMap<EditBookViewModel, EditBookDto>();
    }
}
