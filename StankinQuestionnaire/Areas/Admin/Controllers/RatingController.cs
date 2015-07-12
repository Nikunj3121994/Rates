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
    public class RatingController : Controller
    {
        readonly IAcademicRankRepository _academicRankRepository;

        public RatingController(IAcademicRankRepository academicRankRepository)
        {
            _academicRankRepository = academicRankRepository;
        }

        // GET: Admin/Rating
        public ActionResult Index()
        {
            var ranks = _academicRankRepository.GetRatingsRanksWithGroups();
            var model = Mapper.Map<IEnumerable<AcademicRank>, IEnumerable<AcademicRankViewModel>>(ranks);
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new AcademicRankViewModel();
            model.Groups.Add(new RatingGroupViewModel());
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AcademicRankViewModel model)
        {
            if (ModelState.IsValid)
            {
                var academicRank = Mapper.Map<AcademicRankViewModel, AcademicRank>(model);
                _academicRankRepository.Add(academicRank);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(long id)
        {
            var rank = _academicRankRepository.GetRatingRankWithGroups(id);
            var model = Mapper.Map<AcademicRank, AcademicRankViewModel>(rank);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AcademicRankViewModel model)
        {
            if (ModelState.IsValid)
            {
                var academicRank = Mapper.Map<AcademicRankViewModel, AcademicRank>(model);
                _academicRankRepository.Update(academicRank);
                var groups = Mapper.Map<IEnumerable<RatingGroupViewModel>, IEnumerable<RatingGroup>>(model.Groups);
                _academicRankRepository.UpdateRatings(groups, model.ID);
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