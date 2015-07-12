using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Web.Core.Status;
using StankinQuestionnaire.Models;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DocumentTypeController : AdminController
    {
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IIndicatorGroupService _indicatorGroupService;
        public DocumentTypeController(IDocumentTypeService documentTypeService, IIndicatorGroupService indicatorGroupService)
        {
            this._documentTypeService = documentTypeService;
            this._indicatorGroupService = indicatorGroupService;
        }

        public ActionResult Index()
        {
            var documentTypeViewModel = new DocumentTypeViewModel();
            var documentTypes = _documentTypeService.GetDocumentTypes();
            var documentTypesDetails = Mapper.Map<IEnumerable<DocumentType>, IEnumerable<DocumentTypeFormModel>>(documentTypes);
            var indicatorGroups = _indicatorGroupService.GetIndicatorGroupWithDocumentType();
            foreach (var documentType in documentTypesDetails)
            {
                documentType.IndicatorGroups = indicatorGroups.Where(ct => ct.DocumentType != null
                    && ct.DocumentType.DocumentTypeID == documentType.DocumentTypeID)
                    .Select(ct => new SelectListItem
                    {
                        Value = ct.IndicatorGroupID.ToString(),
                        Text = ct.Name,
                        Selected = ct.DocumentType == null ? false : ct.DocumentType.DocumentTypeID == documentType.DocumentTypeID
                    });
            }
            documentTypeViewModel.DocumentTypes = documentTypesDetails;
            documentTypeViewModel.IndicatorGroupSelect = Mapper.Map<IEnumerable<IndicatorGroup>, IEnumerable<IndicatorGroupSelect>>
                (indicatorGroups.Where(ct => ct.DocumentType == null)).ToList();
            return View(documentTypeViewModel);
        }

        [HttpPost]
        public ActionResult Add(DocumentTypeAddModel addDocumentType)
        {
            var documentType = Mapper.Map<DocumentTypeAddModel, DocumentType>(addDocumentType);
            if (addDocumentType.IndicatorGroupSelect != null)
            {
                documentType.IndicatorsGroups = _indicatorGroupService
                    .GetIndicatorGroups(i => addDocumentType.IndicatorGroupSelect.Contains(i.IndicatorGroupID))
                    .ToList();
            }
            if (ModelState.IsValid)
            {
                _documentTypeService.CreateDocumentType(documentType);
                this.AddStatus(StatusType.SUCCESS,"Успешно добавлен!");
                return RedirectToAction("Index");
            }
            return null;
        }

        [HttpPost]
        public JsonResult Edit(DocumentTypeEditModel editDocumentType)
        {
            var documentType = Mapper.Map<DocumentTypeEditModel, DocumentType>(editDocumentType);
            if (ModelState.IsValid)
            {
                _documentTypeService.EditDocumentType(documentType);
                if (editDocumentType.IndicatorGroups == null)
                {
                    editDocumentType.IndicatorGroups = new List<long>();
                }
                _indicatorGroupService.UpdateDocumentTypes(editDocumentType.IndicatorGroups, documentType.DocumentTypeID);

                editDocumentType.DateChanged = documentType.DateChanged.ToString();
                editDocumentType.DateCreated = documentType.DateCreated.ToString();
                return Json(new EntityJson { Entity = editDocumentType, Text = "Успешно изменен!", Status = EntityStatus.SUCCESS });
            }
            return Json(new EntityJson { Entity = editDocumentType, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }

        [HttpPost]
        public JsonResult Delete(long DocumentTypeID)
        {
            var documentType = _documentTypeService.GetDocumentType(DocumentTypeID);
            if (documentType != null)
            {
                _documentTypeService.DeleteDocumentType(documentType);
                return Json(new IDJson<long> { ID = DocumentTypeID, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = DocumentTypeID, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }
    }
}