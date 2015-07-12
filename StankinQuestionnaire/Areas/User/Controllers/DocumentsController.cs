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
using StankinQuestionnaire.Web.Core.Enums;
using StankinQuestionnaire.Data.Models;

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
        readonly ICheckedRepository _checkRepository;
        readonly IDocumentLogRepository _docLogRepository;
        readonly ICalculationRepository _calcRepository;

        public DocumentsController(IDocumentService documentService, ICalculationService calculationService, IUserService userService, IDocumentTypeRepository documentTypeRepository, IDocumentRepository documentRepository, IUserRepository userRepository, ICheckedRepository checkRepository, IDocumentLogRepository documentLogRepository, ICalculationRepository calculationRepository)
        {
            _documentService = documentService;
            _calculationService = calculationService;
            _userService = userService;
            _documentTypeRepository = documentTypeRepository;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _checkRepository = checkRepository;
            _docLogRepository = documentLogRepository;
            _calcRepository = calculationRepository;
        }


        // GET: User/Documents
        public ActionResult Index()
        {
            long userID = User.Identity.GetUserId<long>();
            var oldDocuments = _documentRepository.GetOld(userID);
            var documentTypes = _documentTypeRepository.GetActive();
            var documentsViewModel = new DocumentsViewModel(oldDocuments, documentTypes);
            //documentsViewModel.AllowDouments = _documentTypeRepository.GetAllowForCreateDocument(userID);
            return View(documentsViewModel);
        }

        public ActionResult Document(long id, int year, EditMode? mode, long? creatorID)
        {
            Document currentDocument = null;
            long currentUserID = User.Identity.GetUserId<long>();

            if (!creatorID.HasValue)//если обычный пользователь, то его id и есть id создателя
            {
                creatorID = currentUserID;
            }

            if (_documentRepository.IsDocumentAllowUser(mode, id, currentUserID))
            {
                currentDocument = _documentRepository.GetByYear(id, creatorID.Value, year);
                if (currentDocument == null)
                {
                    var documentType = _documentTypeRepository.GetById(id);
                    if (documentType != null)
                    {
                        var creator = _userRepository.GetById(creatorID.Value);
                        currentDocument = new Document
                        {
                            DocumentType = documentType,
                            DateCreated = DateTime.Now,
                            DateChanged = DateTime.Now,
                            Creator = creator
                        };
                        _documentRepository.Add(currentDocument);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }

                var documentConstructor = new DocumentConstructor
                {
                    DocumentID = currentDocument.DocumentID,
                    MaxPoint = currentDocument.DocumentType.MaxPoint,
                    Name = currentDocument.DocumentType.Name,
                    Mode = mode,
                    CreatorID = creatorID.Value
                };

                return View(documentConstructor);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public JsonResult DocumentJSON(long documentID, EditMode? mode)
        {
            if (mode != null && mode == EditMode.Checker)
            {
                long checkerID = User.Identity.GetUserId<long>();
                var currentDocument = _documentRepository.GetDocumentWithAllForChecked(documentID, checkerID);
                return Json(currentDocument, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var currentDocument = _documentService.GetFullDocument(documentID);
                var documentJSON = Mapper.Map<Document, DocumentJSON>(currentDocument);
                documentJSON.SetCalculations(currentDocument.Calculations);
                documentJSON.InitCalculations();
                return Json(documentJSON, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddCalculation(SaveCalculation calculation)
        {
            long userID = User.Identity.GetUserId<long>();
            var user = _userService.GetUser(userID);

            Calculation calc = new Calculation
            {
                CalculationTypeID = calculation.CalculationTypeID,
                Description = calculation.Description,
                DocumentID = calculation.DocumentID
            };
            _calculationService.AddCalculation(calc);
            if (!_documentRepository.IsOwner(calc.DocumentID, userID))
            {
                _docLogRepository.AddAdded(userID, calc.CalculationID);
            }
            calculation.CalculationID = calc.CalculationID;
            return Json(calculation);
        }

        public JsonResult UpdateCalculation(CalculationJSON calculation)
        {
            long userID = User.Identity.GetUserId<long>();
            var calculationDesc = _calcRepository.GetById(calculation.CalculationID).Description;
            var calculationDB = Mapper.Map<CalculationJSON, Calculation>(calculation);
            if (_calculationService.CalculationAlowUser(calculation.CalculationID, userID))
            {
                _calculationService.UpdateCalculation(calculationDB);

                if (!_calcRepository.IsOwner(calculation.CalculationID, userID))
                {
                    _docLogRepository.AddUpdated(userID, calculation.CalculationID, calculationDesc);
                }
            }
            return Json(calculation);
        }

        public JsonResult DeleteCalculation(long calculationID)
        {
            long userID = User.Identity.GetUserId<long>();
            if (_calculationService.CalculationAlowUser(calculationID, userID))
            {
                var calculation = _calcRepository.GetById(calculationID);
                var documentId = calculation.DocumentID;
                var description = calculation.Description;
                _calculationService.DeleteCalculation(calculationID);

                if (!_calcRepository.IsOwner(calculationID, userID))
                {
                    _docLogRepository.AddDeleted(userID, documentId, description);
                }
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
        }

        [HttpPost]
        public JsonResult ChangeCheck(CheckedEditModel checkModel)
        {
            _checkRepository.SetCheck(checkModel.DocumentID, checkModel.IndicatorGroupID, checkModel.Checked);
            return Json("");
        }
    }
}