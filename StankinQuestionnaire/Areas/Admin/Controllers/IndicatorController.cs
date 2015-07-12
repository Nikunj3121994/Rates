using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Service;
using StankinQuestionnaire.Areas.Admin.Models;
using AutoMapper;
using StankinQuestionnaire.Models;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Web.Core.Status;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class IndicatorController : Controller
    {
        private IIndicatorService _indicatorService;
        private ICalculationTypeService _calculationTypeService;
        public IndicatorController(IIndicatorService indicatorService, ICalculationTypeService calculationTypeService)
        {
            this._indicatorService = indicatorService;
            this._calculationTypeService = calculationTypeService;
        }

        public ActionResult Index()
        {
            var indicatorViewModel = new IndicatorViewModel();
            var indicators = _indicatorService.GetIndicators();
            var indicatorsDetails = Mapper.Map<IEnumerable<Indicator>, IEnumerable<IndicatorFormModel>>(indicators);
            var calculationTypes = _calculationTypeService.GetCalculationsTypeWithIndicator();
            foreach (var indicator in indicatorsDetails)
            {
                indicator.CalculationTypes = calculationTypes.Where(ct => ct.Indicator != null
                    && ct.Indicator.IndicatorID == indicator.IndicatorID)
                    .Select(ct => new SelectListItem
                {
                    Value = ct.CalculationTypeID.ToString(),
                    Text = ct.UnitName,
                    Selected = ct.Indicator == null ? false : ct.Indicator.IndicatorID == indicator.IndicatorID
                });
            }
            indicatorViewModel.Indicators = indicatorsDetails;
            indicatorViewModel.CalculationTypeSelect = Mapper.Map<IEnumerable<CalculationType>, IEnumerable<CalculationTypeSelect>>
                (calculationTypes.Where(ct => ct.Indicator == null)).ToList();
            return View(indicatorViewModel);
        }

        [HttpPost]
        public ActionResult Add(IndicatorAddModel addIndicator)
        {
            var indicator = Mapper.Map<IndicatorAddModel, Indicator>(addIndicator);
            if (addIndicator.CalculationTypeSelect != null)
            {
                indicator.CalculationTypes = _calculationTypeService.GetCalculationTypes(ct => addIndicator.CalculationTypeSelect.Contains(ct.CalculationTypeID)).ToList();
            }
            if (ModelState.IsValid)
            {
                _indicatorService.CreateIndicator(indicator);
                this.AddStatus(StatusType.SUCCESS,"Успешно добавлен!");
                return RedirectToAction("Index");
            }
            return null;
        }

        [HttpPost]
        public JsonResult Edit(IndicatorEditModel editIndicator)
        {
            var indicator = Mapper.Map<IndicatorEditModel, Indicator>(editIndicator);
            if (ModelState.IsValid)
            {
                _indicatorService.EditIndicator(indicator);
                if (editIndicator.CalculationTypes == null)
                {
                    editIndicator.CalculationTypes = new List<long>();
                }
                _calculationTypeService.UpdateIndicator(editIndicator.CalculationTypes, indicator.IndicatorID);

                editIndicator.DateChanged = indicator.DateChanged.ToString();
                editIndicator.DateCreated = indicator.DateCreated.ToString();
                return Json(new EntityJson { Entity = editIndicator, Text = "Успешно изменен!", Status = EntityStatus.SUCCESS });
            }
            return Json(new EntityJson { Entity = editIndicator, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }

        [HttpPost]
        public JsonResult Delete(long IndicatorID)
        {
            var indicator = _indicatorService.GetIndicator(IndicatorID);
            if (indicator != null)
            {
                _indicatorService.DeleteIndicator(indicator);
                return Json(new IDJson<long> { ID = IndicatorID, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = IndicatorID, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }
    }
}