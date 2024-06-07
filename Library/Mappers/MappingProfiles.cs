using AutoMapper;
using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Model.Models.Menu;
using Library.Service.Dtos.Authorization;
using Library.Service.Dtos.Email;
using Library.Service.Dtos.Employee;
using Library.Service.Dtos.Publisher;
using Library.ViewModels;

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
    }
}
