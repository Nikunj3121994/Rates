using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Service;

namespace StankinQuestionnaire.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private ICalculationTypeService indicatorservice;

        public HomeController(ICalculationTypeService indicatorservice)
        {
            this.indicatorservice = indicatorservice;
        }
        // GET: User/Home
        public ActionResult Index()
        {
            indicatorservice.UpdateIndicator(new List<long>(), 3);
            return View();
        }
    }
}