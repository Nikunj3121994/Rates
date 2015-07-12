using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class AcademicRankViewModel
    {
        public AcademicRankViewModel()
        {
            Groups = new List<RatingGroupViewModel>();
        }
        public long ID { get; set; }
        [Display(Name = "Название ученой степени")]
        public string Title { get; set; }
        public IList<RatingGroupViewModel> Groups { get; set; }
    }

    public class RatingGroupViewModel
    {
        public long RatingGroupID { get; set; }
        [Display(Name = "Название категории")]
        public string Name { get; set; }
        [Display(Name = "Максимальная граница")]
        public int MaxLimit { get; set; }
        [Display(Name = "Минимальная граница")]
        public int MinLimit { get; set; }
    }
}