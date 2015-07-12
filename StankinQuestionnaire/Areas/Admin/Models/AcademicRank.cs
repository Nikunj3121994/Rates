using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class AcademicRankCatalogViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ParentName { get; set; }
    }

    public class AcademicRankCatalogEditModel
    {
        public long ID { get; set; }
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Ученая степень для расчета рейтинга")]
        public long ParentID { get; set; }
        public IEnumerable<SelectListItem> PossibleParent { get; set; }
    }

}