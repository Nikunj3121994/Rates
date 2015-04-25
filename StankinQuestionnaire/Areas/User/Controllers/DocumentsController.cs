using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Service;
using Microsoft.AspNet.Identity;
using StankinQuestionnaire.Areas.User.Models;
using AutoMapper;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Data.Repository;

namespace StankinQuestionnaire.Areas.User.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly ICalculationService _calculationService;
        private readonly IUserService _userService;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        public DocumentsController(IDocumentService documentService, ICalculationService calculationService, IUserService userService, IDocumentTypeRepository documentTypeRepository, IDocumentRepository documentRepository, IUserRepository userRepository)
        {
            _documentService = documentService;
            _calculationService = calculationService;
            _userService = userService;
            _documentTypeRepository = documentTypeRepository;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
        }


        // GET: User/Documents
        public ActionResult Index()
        {
            long userID = User.Identity.GetUserId<long>();
            var documents = _documentService.GetDocumentsByUser(userID);
            var documentsViewModel = new DocumentsViewModel(documents);
            documentsViewModel.AllowDouments = _documentTypeRepository.GetAllowForCreateDocument(userID);
            return View(documentsViewModel);
        }

        public ActionResult Document(long documentID)
        {
            var documentType = _documentRepository.GetDocumentType(documentID);
            var documentConstructor = new DocumentConstructor
            {
                DocumentID = documentID,
                MaxPoint = documentType.MaxPoint,
                Name = documentType.Name
            };
            return View(documentConstructor);
        }

        [HttpGet]
        public JsonResult DocumentJSON(long documentID)
        {
            long userID = User.Identity.GetUserId<long>();
            var document = _documentService.GetFullDocument(documentID);
            var documentJSON = Mapper.Map<Document, DocumentJSON>(document);
            documentJSON.SetCalculations(document.Calculations);
            documentJSON.InitCalculations();
            return Json(documentJSON, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddCalculation(SaveCalculation calculation)
        {
            long userID = User.Identity.GetUserId<long>();
            var user = _userService.GetUser(userID);

            Calculation calc = new Calculation
            {
                CalculationTypeID = calculation.CalculationTypeID,
                Description = calculation.Description,
                Creator = user,
                DocumentID = calculation.DocumentID
            };
            _calculationService.AddCalculation(calc);
            calculation.CalculationID = calc.CalculationID;
            return Json(calculation);
        }

        public JsonResult UpdateCalculation(CalculationJSON calculation)
        {
            long userID = User.Identity.GetUserId<long>();
            var calculationDB = Mapper.Map<CalculationJSON, Calculation>(calculation);
            if (_calculationService.CalculationAlowUser(calculation.CalculationID, userID))
            {
                _calculationService.UpdateCalculation(calculationDB);
            }
            return Json(calculation);
        }

        public JsonResult DeleteCalculation(long calculationID)
        {
            long userID = User.Identity.GetUserId<long>();
            if (_calculationService.CalculationAlowUser(calculationID, userID))
            {
                _calculationService.DeleteCalculation(calculationID);
                return Json(new { calculationID });
            }
            return Json("");
        }

        public ActionResult CreateDocument(int documentTypeID)
        {
            long userID = User.Identity.GetUserId<long>();
            var user = _userRepository.GetById(userID);
            var documentType = _documentTypeRepository.GetById(documentTypeID);
            if (documentType != null)
            {
                _documentRepository.Add(new Document
                {
                    Creator = user,
                    DocumentType = documentType,
                    DateChanged = DateTime.Now,
                    DateCreated = DateTime.Now
                });
            }
            return RedirectToAction("Index");
            //var documentTypes = _documentService.GetNotFillDocumentType(userID, DateTime.Now);
            //return View(documentTypes);
        }
    }
}