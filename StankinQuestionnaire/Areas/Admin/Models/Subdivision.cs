using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class SubdivisionViewModel
    {
        public long SubdivisionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UsersCount { get; set; }
    }

    public class SubdivisionEditModel
    {
        public long SubdivisionID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Руководители")]
        public IEnumerable<SelectListItem> Users { get; set; }
        public IEnumerable<long> UsersID { get; set; }
    }

    public class SubdivisionRating
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
    }
}