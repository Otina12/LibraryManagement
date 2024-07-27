using AutoMapper;
using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Model.Models.Menu;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Authorization;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Book.Post;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;
using Library.Service.Dtos.Email.Post;
using Library.Service.Dtos.Employee.Get;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Service.Dtos.OriginalBook.Post;
using Library.Service.Dtos.Publisher.Get;
using Library.Service.Dtos.Publisher.Post;
using Library.Service.Dtos.Reservations.Post;
using Library.ViewModels.Authorization;
using Library.ViewModels.Authors;
using Library.ViewModels.Books;
using Library.ViewModels.Customers;
using Library.ViewModels.Emails;
using Library.ViewModels.Employees;
using Library.ViewModels.OriginalBooks;
using Library.ViewModels.Publishers;
using Library.ViewModels.Reservations;
using Library.ViewModels.Shared;

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

        // original book maps
        CreateMap<OriginalBookDto, OriginalBookViewModel>().ReverseMap();
        CreateMap<CreateOriginalBookViewModel, CreateOriginalBookDto>();

        // book maps
        CreateMap<CreateBookViewModel, CreateBookDto>();
        CreateMap<BookDetailsDto, EditBookViewModel>()
            .ForMember(dest => dest.AuthorIds, opt => opt.MapFrom(src => src.AuthorsDto.Select(a => a.Id).ToList()))
            .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.PublisherDto != null ? src.PublisherDto.Id : Guid.Empty));
        CreateMap<EditBookViewModel, EditBookDto>();

        // customer maps
        CreateMap<CustomerDto, CustomerViewModel>().ReverseMap();
        CreateMap<CreateCustomerViewModel, CreateCustomerDto>();

        // reservation maps
        CreateMap<CreateReservationViewModel, CreateReservationDto>();

        CreateMap<BooksReservationViewModel, BooksReservationDto>()
            .ForMember(dest => dest.SupposedReturnDate, opt => opt.MapFrom(src => src.SupposedReturnDate.ToDateTime(new TimeOnly(23, 59))));
    }
}
