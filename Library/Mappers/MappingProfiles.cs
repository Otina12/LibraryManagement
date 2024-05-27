using AutoMapper;
using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Service.Dtos;
using Library.Service.Dtos.Email;
using Library.ViewModels;

namespace Library.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterViewModel, RegisterDto>();

        CreateMap<LoginViewModel, LoginDto>();

        CreateMap<ResetPasswordViewModel, ResetPasswordDto>();

        CreateMap<EmployeeRolesViewModel, EmployeeDto>();

        CreateMap<EditEmailTemplateViewModel, EditEmailDto>();
        CreateMap<EmailModel, EditEmailTemplateViewModel>();
        CreateMap<CreateEmailTemplateViewModel, CreateEmailDto>();
    }
}
