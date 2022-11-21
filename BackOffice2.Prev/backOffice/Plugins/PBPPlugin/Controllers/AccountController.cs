using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecurityPlugin.Models;
using backOffice.Infrastructure;
using backOffice.Infrastructure.Security;
using PIC.Domain;
using PIC.Domain.Abstract;

namespace PBPPlugin.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        private IBackOfficeRepository _backOfficeRepository;

        public AccountController()
        {

        }
        public AccountController(IBackOfficeRepository backOfficeRepository)
        {
            this._backOfficeRepository = backOfficeRepository;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost/*, ValidateInput(false)*/]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var oAccountController = new SecurityPlugin.Controllers.AccountController(_backOfficeRepository);

            return oAccountController.Login(model, returnUrl);
        }
    }
}
