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

namespace StankinQuestionnaire.Areas.User.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly ICalculationService _calculationService;
        private readonly IUserService _userService;
        public DocumentsController(IDocumentService documentService, ICalculationService calculationService, IUserService userService)
        {
            _documentService = documentService;
            _calculationService = calculationService;
            _userService = userService;
        }


        // GET: User/Documents
        public ActionResult Index()
        {
            long userID = User.Identity.GetUserId<long>();
            var documents = _documentService.GetDocumentsByUser(userID);
            var documentsViewModel = new DocumentsViewModel(documents);
            return View(documentsViewModel);
        }

        public ActionResult Document(long documentID)
        {
            return View(documentID);
        }

        [HttpGet]
        public JsonResult DocumentJSON(long documentID)
        {
            long userID = User.Identity.GetUserId<long>();
            var document = _documentService.GetFullDocument(documentID);
            var documentJSON = Mapper.Map<Document, DocumentJSON>(document);
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
                Creator = user
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
            }
            return Json(new { calculationID });
        }

        public ActionResult CreateDocument()
        {
            long userID = User.Identity.GetUserId<long>();
            var documentTypes = _documentService.GetNotFillDocumentType(userID, DateTime.Now);
            return View(documentTypes);
        }
    }
}