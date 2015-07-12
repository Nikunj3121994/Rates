using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class ReviewerController : Controller
    {
        readonly IUserRepository _userRepository;
        readonly IDocumentTypeRepository _documentTypeRepository;
        public ReviewerController(IUserRepository userRepository, IDocumentTypeRepository documentTypeRepository)
        {
            _userRepository = userRepository;
            _documentTypeRepository = documentTypeRepository;
        }
        // GET: Admin/Reviewer
        public ActionResult Index()
        {
            var user = _userRepository.GetReviewers();
            var model = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ReviewerViewModel>>(user);
            return View(model);
        }

        public ActionResult Edit(long id)
        {
            var model = new ReviewerEditModel();
            var documentTypes = _documentTypeRepository.GetWithIndicatorGroups();
            var user = _userRepository.GetById(id);
            model.Id = user.Id;
            model.Name = string.Format("{0} {1} {2}", user.SecondName, user.FirstName, user.MiddleName);
            model.DocumentTypes = Mapper.Map<IEnumerable<DocumentType>, IEnumerable<DocumentTypeSelect>>(documentTypes);
            model.IndicatorGroups = Mapper.Map<IEnumerable<DocumentType>, IEnumerable<IndicatorGroups>>(documentTypes);
            model.SelectedIndicatorGroupsID = _userRepository.GetAllowIndicatorGroups(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ReviewerEditModel model)
        {
            if (ModelState.IsValid)
            {
                _userRepository.UpdateAllowIndicatorGroups(model.Id, model.SelectedIndicatorGroupsID);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}