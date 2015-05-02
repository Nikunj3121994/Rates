using StankinQuestionnaire.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class SubdivisionController : Controller
    {
        readonly ISubdivisionRepository _subvisionRepository;
        public SubdivisionController(ISubdivisionRepository subvisionRepository)
        {
            _subvisionRepository = subvisionRepository;
        }
        // GET: Admin/Subvision
        public ActionResult Index()
        {
            var subvisions = _subvisionRepository.GetSubvisionWithUsers();
            return View(subvisions);
        }
    }
}