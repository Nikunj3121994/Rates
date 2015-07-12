using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Service;
using StankinQuestionnaire.Areas.User.Models;
using StankinQuestionnaire.Web.Core;
using StankinQuestionnaire.Data.Repository;
using Microsoft.AspNet.Identity;
using AutoMapper;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Data.Models;

namespace StankinQuestionnaire.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private ICalculationTypeService indicatorservice;
        readonly ICalculator _calculator;
        readonly IDocumentRepository _documentRepository;
        readonly IUserRepository _userRepository;

        public HomeController(ICalculationTypeService indicatorservice, ICalculator calculator, IDocumentRepository documentRepository,
            IUserRepository userRepository)
        {
            this.indicatorservice = indicatorservice;
            _calculator = calculator;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
        }
        // GET: User/Home
        //public ActionResult Index()
        //{
        //    indicatorservice.UpdateIndicator(new List<long>(), 3);
        //    return View();
        //}

        public ActionResult Rating()
        {
            long userID = User.Identity.GetUserId<long>();
            var groupsDocument = _documentRepository.GetGroupByYear(userID);
            var model = new List<UserRating>();
            var ratingGroups = _userRepository.GetRatingGroups(userID);

            foreach (var group in groupsDocument)
            {
                var currentDocuments = new List<DocumentJSON>();
                foreach (var document in group.ToList())
                {
                    var documentJSON = Mapper.Map<Document, DocumentJSON>(document);
                    documentJSON.SetCalculations(document.Calculations);
                    documentJSON.InitCalculations();
                    currentDocuments.Add(documentJSON);
                }

                var point = _calculator.CalculatePointForTeacher(currentDocuments);
                var ratingGroup = ratingGroups.FirstOrDefault(rg => rg.MinLimit <= point && rg.MaxLimit >= point);
                model.Add(new UserRating
                {
                    Category = ratingGroup != null ? ratingGroup.Name : String.Empty,
                    Point = _calculator.CalculatePointForTeacher(currentDocuments),
                    Year = group.Key
                });
            }
            return View(model);
        }
    }
}