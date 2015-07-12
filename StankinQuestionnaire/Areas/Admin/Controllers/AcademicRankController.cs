using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class AcademicRankController : Controller
    {
        readonly IAcademicRankRepository _academicRankRepository;

        public AcademicRankController(IAcademicRankRepository academicRankRepository)
        {
            _academicRankRepository = academicRankRepository;
        }

        // GET: Admin/AcademicRank
        public ActionResult Index()
        {
            var ranks = _academicRankRepository.GetCatalogAcademicRanks();
            var model = Mapper.Map<IEnumerable<AcademicRank>, IEnumerable<AcademicRankCatalogViewModel>>(ranks);
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new AcademicRankCatalogEditModel();
            model.PossibleParent = _academicRankRepository
                .GetPossibleParents()
                .Select(pp => new SelectListItem
                {
                    Text = pp.Title,
                    Value = pp.AcademicRankID.ToString()
                });
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AcademicRankCatalogEditModel model)
        {
            if (ModelState.IsValid)
            {
                var academicRank = Mapper.Map<AcademicRankCatalogEditModel, AcademicRank>(model);
                _academicRankRepository.Add(academicRank);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(long id)
        {
            var academicRank = _academicRankRepository.GetById(id);
            var model = Mapper.Map<AcademicRank, AcademicRankCatalogEditModel>(academicRank);
            model.PossibleParent = _academicRankRepository
                .GetPossibleParents()
                .Select(pp => new SelectListItem
                {
                    Text = pp.Title,
                    Value = pp.AcademicRankID.ToString()
                });
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AcademicRankCatalogEditModel model)
        {
            if (ModelState.IsValid)
            {
                var academicRank = Mapper.Map<AcademicRankCatalogEditModel, AcademicRank>(model);
                _academicRankRepository.UpdateWithParent(academicRank);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(long ID)
        {
            var academicRank = _academicRankRepository.GetById(ID);
            _academicRankRepository.Delete(academicRank);
            return Json(new IDJson<long> { ID = ID, Status = EntityStatus.SUCCESS, Text = "Успешно удалено!" });
        }
    }
}