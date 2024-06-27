using AutoMapper;
using Library.Model.Enums;
using Library.Service.Dtos.Email;
using Library.Service.Interfaces;
using Library.Attributes.Authorization;
using Library.ViewModels.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class EmailController : BaseController
    {
        public EmailController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
        {
        }


        [CustomAuthorize(nameof(Role.Admin))] // custom attribute to move users to pages when they have no required role
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var emailTemplates = await _serviceManager.EmailService.GetAllTemplates();
            return View(emailTemplates);
        }


        [CustomAuthorize(nameof(Role.Admin))]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [CustomAuthorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmailTemplateViewModel emailTemplateVM)
        {
            if (!ModelState.IsValid)
            {
                CreateFailureNotification($"Unable to create an email template '{emailTemplateVM.Subject}'");
                return View(emailTemplateVM);
            }

            var result = await _serviceManager.EmailService.Create(_mapper.Map<CreateEmailDto>(emailTemplateVM));

            return HandleResult(result, emailTemplateVM,
                $"Successfully created an email template '{emailTemplateVM.Subject}'",
                result.Error.Message,
                controllerName: "Email");
        }


        [CustomAuthorize(nameof(Role.Admin))]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var emailTemplate = await _serviceManager.EmailService.GetEmail(Id);
            return View(_mapper.Map<EditEmailTemplateViewModel>(emailTemplate));
        }

        [CustomAuthorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmailTemplateViewModel emailTemplateVM)
        {
            if(!ModelState.IsValid)
            {
                CreateFailureNotification($"Unable to edit an email template '{emailTemplateVM.Subject}'");
                return View(emailTemplateVM);
            }

            await _serviceManager.EmailService.Update(_mapper.Map<EditEmailDto>(emailTemplateVM));
            CreateSuccessNotification($"Successfully edited an email template '{emailTemplateVM.Subject}'");
            return View();
        }

        [CustomAuthorize("Admin")]
        [HttpDelete("Email/Delete/{id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _serviceManager.EmailService.Delete(Id);

            if (result.IsFailure)
            {
                CreateFailureNotification("Unable to delete given email template");
            }
            else
            {
                CreateSuccessNotification("Successfully deleted an email template");
            }

            return HandleResult(result, null,
                "Successfully deleted an email template",
                result.Error.Message,
                controllerName: "Email"
                );
        }

    }
}
