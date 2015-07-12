using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using StankinQuestionnaire.Data.Repository;
using AutoMapper;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Areas.Admin.Models;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class CheckController : Controller
    {
        readonly ICheckedRepository _checkRepository;
        public CheckController(ICheckedRepository checkRepository)
        {
            _checkRepository = checkRepository;
        }
        // GET: Admin/Check
        public ActionResult Index()
        {
            var model = new CheckedViewModel();
            long userID = User.Identity.GetUserId<long>();
            var documents = _checkRepository.GetDocuments(userID);
            var groups = Mapper.Map<IEnumerable<DocumentsWithMaxCheckedCountDTO>, IEnumerable<GroupDocuments>>(documents);
            model.Groups = groups;
            return View(model);
        }
    }
}