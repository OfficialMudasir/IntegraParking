using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Web.Routing;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Models;
using integraMobile.Web.Resources;
using System.Configuration;


namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class HomeController : Controller
    {

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;


        public HomeController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

       
        public ActionResult Index()
        {            
            return View();
        }
              
    }
}
