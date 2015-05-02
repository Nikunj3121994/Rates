using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Helper;
using StankinQuestionnaire.Model;
using Microsoft.AspNet.Identity;
using StankinQuestionnaire.Web.Core.Status;
using StankinQuestionnaire.Service;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser, long> _userManager;
        readonly IUserRepository _userRepository;
        readonly ISubdivisionRepository _subdivisionRepository;

        public UserController(IUserRepository userRepository, ISubdivisionRepository subdivisionRepository, UserManager<ApplicationUser, long> userManager)
        {
            _subdivisionRepository = subdivisionRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            var users = _userRepository.GetUsersWithSubvision();
            var model = Mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(model);
        }

        public ActionResult Add()
        {
            UserEditModel model = new UserEditModel();
            var subdivisions = _subdivisionRepository.GetMany();
            model.Subdivisions = subdivisions.ToSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = Mapper.Map<UserEditModel, ApplicationUser>(model);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(model.Password);
                _userRepository.Add(user);
                this.AddStatus(StatusType.SUCCESS, "Пользователь успешно добавлен!");
                return RedirectToAction("Index");
            }
            this.AddStatus(StatusType.ERROR, "Произошла ошибка!");
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                return Json(new IDJson<long> { ID = id, Text = "Успешно удален!", Status = EntityStatus.SUCCESS });
            }
            return Json(new IDJson<long> { ID = id, Text = "Вы ввели не правильные данные!", Status = EntityStatus.ERROR });
        }
    }
}