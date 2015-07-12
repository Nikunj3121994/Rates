using AutoMapper;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Data.Models;

namespace StankinQuestionnaire.Areas.Admin.Controllers
{
    public class DocumentLogController : Controller
    {
        readonly IDocumentLogRepository _documentLogRepository;
        public DocumentLogController(IDocumentLogRepository documentLogRepository)
        {
            _documentLogRepository = documentLogRepository;
        }

        public ActionResult Index()
        {
            var documentLogs = _documentLogRepository.GetLogs();
            var model = Mapper.Map<IEnumerable<DocumentLogDTO>, IEnumerable<DocumentLogViewModel>>(documentLogs);
            return View(model);
        }
    }
}