using AutoMapper;
using Library.Model.Enums;
using Library.Model.Models.Email;
using Library.Service.Dtos.Email;
using Library.Service.Interfaces;
using Library.ViewModels;
using Library.ViewModels.Attributes.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class EmailController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public EmailController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
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
                return View(emailTemplateVM);
            }

            await _serviceManager.EmailService.Create(_mapper.Map<CreateEmailDto>(emailTemplateVM));
            return RedirectToAction("Index", "Email");
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
                return View(emailTemplateVM);
            }

            await _serviceManager.EmailService.Update(_mapper.Map<EditEmailDto>(emailTemplateVM));
            return View();
        }

        [CustomAuthorize("Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await _serviceManager.EmailService.Delete(Id);
            return RedirectToAction("Index", "Email");
        }


    }
}
