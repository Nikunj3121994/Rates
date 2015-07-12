using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Service;
using StankinQuestionnaire.Areas.Admin.Models;
using AutoMapper;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Models;
using StankinQuestionnaire.Web.Core.Status;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class IndicatorGroupController : Controller
    {
        private readonly IIndicatorGroupService _indicatorGroupService;
        private readonly IIndicatorService _indicatorService;
        public IndicatorGroupController(IIndicatorGroupService indicatorGroupService, IIndicatorService indicatorService)
        {
            this._indicatorGroupService = indicatorGroupService;
            this._indicatorService = indicatorService;
        }

        public ActionResult Index()
        {
            var indicatorGroupViewModel = new IndicatorGroupViewModel();
            var indicatorGroups = _indicatorGroupService.GetIndicatorGroups();
            var indicatorGroupsDetails = Mapper.Map<IEnumerable<IndicatorGroup>, IEnumerable<IndicatorGroupFormModel>>(indicatorGroups);
            var indicators = _indicatorService.GetIndicatorWithIndicatorGroup();
            foreach (var indicatorGroup in indicatorGroupsDetails)
            {
                indicatorGroup.Indicators = indicators.Where(ct => ct.IndicatorGroup != null
                    && ct.IndicatorGroup.IndicatorGroupID == indicatorGroup.IndicatorGroupID)
                    .Select(ct => new SelectListItem
                {
                    Value = ct.IndicatorID.ToString(),
                    Text = ct.Name,
                    Selected = ct.IndicatorGroup == null ? false : ct.IndicatorGroup.IndicatorGroupID == indicatorGroup.IndicatorGroupID
                });
            }
            indicatorGroupViewModel.IndicatorGroups = indicatorGroupsDetails;
            indicatorGroupViewModel.IndicatorSelect = Mapper.Map<IEnumerable<Indicator>, IEnumerable<IndicatorSelect>>
                (indicators.Where(ct => ct.IndicatorGroup == null)).ToList();
            return View(indicatorGroupViewModel);
        }

        [HttpPost]
        public ActionResult Add(IndicatorGroupAddModel addIndicatorGroup)
        {
            var IndicatorGroup = Mapper.Map<IndicatorGroupAddModel, IndicatorGroup>(addIndicatorGroup);
            if (addIndicatorGroup.IndicatorSelect != null)
            {
                IndicatorGroup.Indicators = _indicatorService.GetIndicators(i => addIndicatorGroup.IndicatorSelect.Contains(i.IndicatorID)).ToList();
            }
            if (ModelState.IsValid)
            {
                _indicatorGroupService.CreateIndicatorGroup(IndicatorGroup);
                this.AddStatus(StatusType.SUCCESS,"Успешно добавлен!");
                return RedirectToAction("Index");
            }
            return null;
        }

        [HttpPost]
        public JsonResult Edit(IndicatorGroupEditModel editIndicatorGroup)
        {
            var indicatorGroup = Mapper.Map<IndicatorGroupEditModel, IndicatorGroup>(editIndicatorGroup);
            if (ModelState.IsValid)
            {
                _indicatorGroupService.EditIndicatorGroup(indicatorGroup);
                if (editIndicatorGroup.Indicators == null)
                {
                    editIndicatorGroup.Indicators = new List<long>();
                }
                _indicatorService.UpdateIndicatorGroups(editIndicatorGroup.Indicators, indicatorGroup.IndicatorGroupID);

                editIndicatorGroup.DateChanged = indicatorGroup.DateChanged.ToString();
                editIndicatorGroup.DateCreated = indicatorGroup.DateCreated.ToString();
                return Json(new EntityJson { Entity = editIndicatorGroup, Text = "Успешно изменен!", Status = EntityStatus.SUCCESS });
            }
            return Json(new EntityJson { Entity = editIndicatorGroup, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }

        [HttpPost]
        public JsonResult Delete(long IndicatorGroupID)
        {
            var indicatorGroup =  _indicatorGroupService.GetIndicatorGroup(IndicatorGroupID);
            if (indicatorGroup != null)
            {
                _indicatorGroupService.DeleteIndicatorGroup(indicatorGroup);
                return Json(new IDJson<long> { ID = IndicatorGroupID, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = IndicatorGroupID, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });//TODO возможен баг
        }
    }
}