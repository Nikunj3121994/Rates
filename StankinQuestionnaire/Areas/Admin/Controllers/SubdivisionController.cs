using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Helper;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Web.Core.Status;
using StankinQuestionnaire.Models;
using Microsoft.AspNet.Identity;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class SubdivisionController : Controller
    {
        readonly ISubdivisionRepository _subvisionRepository;
        readonly IUserRepository _userRepository;
        public SubdivisionController(ISubdivisionRepository subvisionRepository, IUserRepository userRepository)
        {
            _subvisionRepository = subvisionRepository;
            _userRepository = userRepository;
        }
        // GET: Admin/Subvision
        public ActionResult Index()
        {
            var subdivisions = _subvisionRepository.GetSubvisionWithUsers();
            var model = Mapper.Map<IEnumerable<SubdivisionWithUsersCount>, IEnumerable<SubdivisionViewModel>>(subdivisions);
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new SubdivisionEditModel();
            var users = _userRepository.GetMany();
            model.Users = users.ToSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(SubdivisionEditModel model)
        {
            if (ModelState.IsValid)
            {
                Subdivision subdivision = Mapper.Map<SubdivisionEditModel, Subdivision>(model);
                _subvisionRepository.Add(subdivision);
                _subvisionRepository.SetDirectors(model.UsersID, subdivision.SubdivisionID);
                this.AddStatus(StatusType.SUCCESS, "Пользователь успешно добавлен!");
                return RedirectToAction("Index");
            }
            this.AddStatus(StatusType.ERROR, "Произошла ошибка!");
            return View(model);
        }

        public ActionResult Edit(long subdivisionId)
        {
            var subdivision = _subvisionRepository.GetById(subdivisionId);
            var model = Mapper.Map<Subdivision, SubdivisionEditModel>(subdivision);

            var users = _userRepository.GetMany();
            model.Users = users.ToSelectList();
            foreach (var director in model.UsersID)
            {
                model.Users.SetSelect(director.ToString());
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SubdivisionEditModel model)
        {
            if (ModelState.IsValid)
            {
                var subdivision = Mapper.Map<SubdivisionEditModel, Subdivision>(model);
                _subvisionRepository.UpdateWithDirectors(subdivision, model.UsersID);
                this.AddStatus(StatusType.SUCCESS, "Подразделение успешно изменено!");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var subdivision = _subvisionRepository.GetById(id);
            if (subdivision != null)
            {
                _subvisionRepository.Delete(subdivision);
                return Json(new IDJson<long> { ID = id, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = id, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }

        public ActionResult Users()
        {
            long userID = User.Identity.GetUserId<long>();
            var users = _userRepository.GetForSubdivision(userID);
            var model = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(users);
            return View(model);
        }

        public ActionResult Rating()
        {
            var model = new List<SubdivisionRating>(){
                new SubdivisionRating{Name="Кафедра высшей математики",Point=389,Position=1},
                new SubdivisionRating{Name="Кафедра общей физики ",Point=369,Position=2},
                new SubdivisionRating{Name="Кафедра физического воспитания и спорта",Point=339,Position=3}
            };
            return View(model);
        }
    }
}