using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Service;
using StankinQuestionnaire.Areas.Admin.Models;
using AutoMapper;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Web.Core;
using StankinQuestionnaire.Web.Core.Status;
using StankinQuestionnaire.Models;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CalculationTypeController : Controller
    {
        private ICalculationTypeService _calculationTypeService;
        public CalculationTypeController(ICalculationTypeService calculationTypeService)
        {
            this._calculationTypeService = calculationTypeService;
        }

        // GET: Admin/CalculationType
        public ActionResult Index()
        {
            var calculationTypeViewModel = new CalculationTypeViewModel();
            var calculationTypes = _calculationTypeService.GetCalculationTypes();
            var calculationTypesDetails = Mapper.Map<IEnumerable<CalculationType>, IEnumerable<CalculationTypeFormModel>>(calculationTypes);
            calculationTypeViewModel.CalculationTypes = calculationTypesDetails;
            return View(calculationTypeViewModel);
        }

        [HttpPost]
        public ActionResult Add(CalculationTypeAddModel addCalculationType)
        {
            var calculationType = Mapper.Map<CalculationTypeAddModel, CalculationType>(addCalculationType);
            if (ModelState.IsValid)
            {
                _calculationTypeService.CreateCalculationType(calculationType);
                this.AddStatus(StatusType.SUCCESS, "Успешно добавлен!");
                return RedirectToAction("Index");
            }
            return null;
        }

        [HttpPost]
        public JsonResult Edit(CalculationTypeFormModel editCalculationType)
        {
            //var calculationType = Mapper.Map<CalculationTypeFormModel, CalculationType>(editCalculationType);
            // var returnCalculationType = Mapper.Map<CalculationType, CalculationTypeEditModel>(calculationType);
            var calculationTypeDB = _calculationTypeService.GetCalculationType(editCalculationType.CalculationTypeID);
            calculationTypeDB.Point = editCalculationType.Point;
            calculationTypeDB.UnitName = editCalculationType.UnitName;
            calculationTypeDB.MaxPoint = editCalculationType.MaxPoint;
            if (ModelState.IsValid)
            {
                _calculationTypeService.EditCalculationType(calculationTypeDB);
                editCalculationType.DateChanged = calculationTypeDB.DateChanged.ToString();
                editCalculationType.DateCreated = calculationTypeDB.DateCreated.ToString();
                return Json(new EntityJson { Entity = editCalculationType, Text = "Успешно изменен!", Status = EntityStatus.SUCCESS });
            }
            return Json(new EntityJson { Entity = editCalculationType, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }

        [HttpPost]
        public JsonResult Delete(long CalculationTypeID)
        {
            var calculationType = _calculationTypeService.GetCalculationType(CalculationTypeID);
            if (calculationType != null)
            {
                _calculationTypeService.DeleteCalculationType(calculationType);
                return Json(new IDJson<long> { ID = CalculationTypeID, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = CalculationTypeID, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }
    }
}