using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Helper;
using StankinQuestionnaire.Model;
using Microsoft.AspNet.Identity;
using StankinQuestionnaire.Web.Core.Status;
using StankinQuestionnaire.Service;
using System.Web.Helpers;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.IO;
using StankinQuestionnaire.Areas.User.Models;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Web.Core;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser, long> _userManager;
        readonly IUserRepository _userRepository;
        readonly ISubdivisionRepository _subdivisionRepository;
        readonly IDocumentService _documentService;
        readonly IDocumentRepository _documentRepository;
        readonly IDocumentTypeRepository _documentTypeRepository;
        readonly ICalculator _calculator;

        public UserController(IUserRepository userRepository, ISubdivisionRepository subdivisionRepository, UserManager<ApplicationUser, long> userManager,
            IDocumentService documentService, IDocumentRepository documentRepository, IDocumentTypeRepository documentTypeRepository,ICalculator calculator)
        {
            _subdivisionRepository = subdivisionRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _documentService = documentService;
            _documentRepository = documentRepository;
            _documentTypeRepository = documentTypeRepository;
            _calculator = calculator;
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            var users = _userRepository.GetUsersWithSubvision();
            var model = Mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(model);
        }

        public ActionResult Add()
        {
            UserEditModel model = new UserEditModel();
            var subdivisions = _subdivisionRepository.GetMany();
            model.Subdivisions = subdivisions.ToSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = Mapper.Map<UserEditModel, ApplicationUser>(model);
                _userRepository.Add(user);
                if (model.IsAdmin)
                    _userManager.AddToRole(user.Id, Roles.Admin.ToString());
                if (model.IsChecker)
                    _userManager.AddToRole(user.Id, Roles.Checker.ToString());
                this.AddStatus(StatusType.SUCCESS, "Пользователь успешно добавлен!");
                return RedirectToAction("Index");
            }
            this.AddStatus(StatusType.ERROR, "Произошла ошибка!");
            return View(model);
        }

        public ActionResult Edit(long id)
        {
            var user = _userRepository.GetById(id);
            var model = Mapper.Map<ApplicationUser, UserEditModel>(user);
            model.IsChecker = _userManager.IsInRole(user.Id, Roles.Checker.ToString());
            model.IsAdmin = _userManager.IsInRole(user.Id, Roles.Admin.ToString());
            var subdivisions = _subdivisionRepository.GetMany();
            model.Subdivisions = subdivisions.ToSelectList();
            if (user.SubdivisionID != null)
            {
                model.Subdivisions.SetSelect(user.SubdivisionID.Value.ToString());
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<UserEditModel, ApplicationUser>(model);
                if (model.SubvisionID != null)
                {
                    user.SubdivisionID = model.SubvisionID;
                }
                _userRepository.Update(user);
                this.AddStatus(StatusType.SUCCESS, "Пользователь успешно изменен!");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                return Json(new IDJson<long> { ID = id, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = id, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }

        public ActionResult AddFromFile()
        {
            var model = new UsersFromFile();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFromFile(UsersFromFile usersFile)
        {
            if (ModelState.IsValid)
            {
                byte[] array;
                string csv = string.Empty;
                using (MemoryStream ms = new MemoryStream())
                {
                    usersFile.File.InputStream.CopyTo(ms);
                    array = ms.ToArray();
                }
                switch (usersFile.EncodeType)
                {
                    case EncodeType.UTF8:
                        csv = Encoding.UTF8.GetString(array);
                        break;
                    case EncodeType.UTF32:
                        csv = Encoding.UTF32.GetString(array);
                        break;
                    case EncodeType.ASCII:
                        csv = Encoding.ASCII.GetString(array);
                        break;
                }
                var userParser = new UsersParser(csv);
                var usersEM = userParser.ParseUsers();
                if (userParser.Errors.Count() == 0)
                {
                    var users = Mapper.Map<IEnumerable<UserEditModel>, IEnumerable<ApplicationUser>>(usersEM);
                    _userRepository.AddRange(users);
                    this.AddStatus(StatusType.SUCCESS, "Пользователи успешно добавлены!");
                    return RedirectToAction("Index");
                }
            }
            return View(usersFile);
        }

        public ActionResult Documents(long id)
        {
            var oldDocuments = _documentRepository.GetOld(id);
            var documentTypes = _documentTypeRepository.GetActive();
            var documentsViewModel = new DocumentsViewModel(oldDocuments, documentTypes);
            documentsViewModel.CreatorID = id;
            return View(documentsViewModel);
        }

        public ActionResult Rating(long id)
        {
            long userID = id;
            var groupsDocument = _documentRepository.GetGroupByYear(userID);
            var model = new List<UserRating>();
            var ratingGroups = _userRepository.GetRatingGroups(userID);

            foreach (var group in groupsDocument)
            {
                var currentDocuments = new List<DocumentJSON>();
                foreach (var document in group.ToList())
                {
                    var documentJSON = Mapper.Map<Document, DocumentJSON>(document);
                    documentJSON.SetCalculations(document.Calculations);
                    documentJSON.InitCalculations();
                    currentDocuments.Add(documentJSON);
                }

                var point = _calculator.CalculatePointForTeacher(currentDocuments);
                var ratingGroup = ratingGroups.FirstOrDefault(rg => rg.MinLimit <= point && rg.MaxLimit >= point);
                model.Add(new UserRating
                {
                    Category = ratingGroup != null ? ratingGroup.Name : String.Empty,
                    Point = _calculator.CalculatePointForTeacher(currentDocuments),
                    Year = group.Key
                });
            }
            return View(model);
        }
    }
}